var express  = require('express');
var app = express();
var mongoose = require('mongoose');
var port = process.env.PORT || 8080;
var http = require('http');

var database = {
	url : 'mongodb://mongo:27017'
}

var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');

// To be redesigned with a loop and a break on total timeout or number of tries
mongoose.connect(database.url, function(err) {
	if(err) {
		console.log('connection error (first try)', err);
		setTimeout(function() {
			mongoose.connect(database.url, function(err) {
				if(err) {
					console.log('connection error (second try)', err);
					setTimeout(function() {
						mongoose.connect(database.url, function(err) {
							if(err) {
								console.log('connection error (three strikes... you are out)', err);
							} else {
								console.log('successful connection (third try... almost out)');
							}
						});
					},10000);
				} else {
					console.log('successful connection (second try)');
				}
			});
		},5000);
	} else {
		console.log('successful connection (first try)');
	}
});

app.use(express.static(__dirname + '/public'));
app.use(morgan('dev'));
app.use(bodyParser.json()); // parse application/json
app.use(methodOverride('X-HTTP-Method-Override'));

var mongoose = require('mongoose');
var MortgageModel = mongoose.model('Mortgage', {
	text: {type: String, default: ''},
	startMonth: {type: Number},
	repaymentLengthInMonths: {type: Number},
	amountBorrowed: {type: Number},
	isRateFixed: {type: Boolean},
	fixRate: {type: Number},
});

function getMortgages(res){
	MortgageModel.find(function(err, mortgages) {
		if (err)
			res.send(err)
		res.json(mortgages);
	});
};

app.get('/api/mortgages', function(req, res) {
	getMortgages(res);
});
app.post('/api/report', function(req, res) {
	MortgageModel.findOne({ 'text': req.body.reference }, function(err, mortgage) {
		var contentString = JSON.stringify(mortgage);
		var headers = {
			'Content-Type': 'application/json',
			'Content-Length': contentString.length
		};
		var options = {
		  host: 'notifier',
		  port: 5000,
		  path: '/repayment?email=' + req.body.email + '&repaid=' + req.body.repaid + '&newRate=' + req.body.newRate,
		  method: 'POST',
		  headers: headers
		};
		var repaymentRequest = http.request(options);
		repaymentRequest.write(contentString);
		repaymentRequest.end();
	});
});
app.post('/api/mortgages', function(req, res) {
	MortgageModel.create({
		text : req.body.text,
		startMonth: req.body.start,
		repaymentLengthInMonths: req.body.duration,
		amountBorrowed: req.body.amount,
		isRateFixed: req.body.isRateFixed,
		fixRate: req.body.rate
	}, function(err, mortgage) {
		if (err)
			res.send(err);
		getMortgages(res);
	});
});
app.get('*', function(req, res) {
	res.sendfile('./public/index.html');
});

app.listen(port);
console.log("App listening on port " + port);
