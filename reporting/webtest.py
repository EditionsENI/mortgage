from flask import Flask
from flask import make_response
from flask import request
from reportlab.pdfgen import canvas

import requests, json

app = Flask(__name__)

@app.route('/pdf', methods=['POST'])
def pdf():

    # Calling mortgage optimizer (in order to decouple the services, this would be done in a message queue)
    entetes = {'Content-type': 'application/json', 'Accept': 'text/plain'}
    callURL = "http://optimizer"
    callURL += "/api/Mortgage/BestRepaymentDate"
    callURL += "?AmountRepaid=" + request.args['amountRepaid']
    callURL += "&ReplacementFixRate=" + request.args['replacementFixRate']
    r = requests.post(callURL, data=request.data, headers=entetes)
    print("Best repayment date is : " + r.content.decode('utf-8'))

    # Generating PDF report
    import io
    output = io.BytesIO()
    p = canvas.Canvas(output)
    p.drawString(100, 100, 'Best date to reimburse your mortage is : ' + r.content.decode('utf-8'))
    p.showPage()
    p.save()   
    pdf_out = output.getvalue()
    output.close()
 
    # Sending PDF as a MIME content
    response = make_response(pdf_out)
    response.headers['Content-Disposition'] = "attachment; filename=report.pdf"
    response.mimetype = 'application/pdf'
    return response

if __name__ == '__main__':
    app.run(host='0.0.0.0')
