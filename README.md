# mortgage
This same application has been created as an example for Editions ENI's book on Docker, by Jean-Philippe Gouigoux (ISBN : 978-2-7460-9752-0).

## Business domain 
The application simulates a very simple mortgage repayment optimization tool. After creating a mortgage, which will be stored in a NoSQL database, the GUI lets the user requests the analysis of the best repayment month, depending on the capital she can reimburse and the new interest rate. A PDF document is then sent to her with the corresponding date.

## Architecture
This sample application has been created to illustrate a (hugely simplified) microservices architecture. The simple use case described above is handled by six microservices :

- **portal** : the GUI that enables creation of a mortage definition and request for a report about best repayment strategy;
- **mongo** : the database used for persistance of the mortgages;
- **notifier** : the service that handles the sending of the message to the requester;
- **reporting** : the service in charge of generating the PDF document;
- **optimizer** : it runs the many jobs corresponding to the possible reimbursement dates and then selects which one it the best;
- **calculator** : these instances computes the total cost of the mortgage for all the cases sent by the previous service, and send it a message stating the result for each job, until there are not any ones to compute.

In order to show different ways to use Docker with various languages, the services are created with several languages :

- portal uses Node.js and Angular.js, with Mongoose and Express;
- notifier uses Java, together with the Spark framework;
- reporting is written in Python, with Flask and Reportlab;
- optimizer and calculator use .NET Core / ASP.NET 5 / MVC 6.   

## Use

Easiest way to run the application is to use the docker-compose.yml file that is provided :

    git clone https://github.com/EditionsENI/mortgage.git
    cd mortgage
    cd aspnetbase
    docker build -t aspnetbase:beta4 .
    cd ..
    cd nodebase
    docker build -t nodebase .
    cd ..
    docker-compose build
    export SMTP_AUTH_LOGIN=your.identifier@gmail.com
    export SMTP_AUTH_PASSWORD=your.gmail.password
    docker-compose up

The application is then available under 
> http://localhost:8080

## Notes

Gmail credentiels are necessary for mail sending. If not provided, the rest of the application will run, but the report will not be sent. If another SMTP provider should be used, please modify MailSender.java in the notifier service accordingly.