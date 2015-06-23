package sparkexample;

import static spark.Spark.*;
import java.io.File;
import java.io.InputStream;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.net.URL;
import java.net.HttpURLConnection;

public class Hello {

    public static void main(String[] args) {
        setPort(5000);

        post("/repayment", (request, response) -> {

	    // It is best to check that the variables used to send mail are set before calculating the report,
	    // in order not to lose calculation time if they are missing and the report cannot be sent anyway
	    final String username = System.getenv("SMTP_AUTH_LOGIN");
	    final String password = System.getenv("SMTP_AUTH_PASSWORD");
 
	    if (username == null || username.length() == 0) {
		System.out.println("ERROR : no SMTP username has been set in environment variable SMTP_AUTH_LOGIN");
		return "ERROR";
	    }
            System.out.println("Account used to send mail is " + username);
	    if (password == null || password.length() == 0) {
		System.out.println("ERROR : no SMTP password has been set in environment variable SMTP_AUTH_PASSWORD");
		return "ERROR";
	    }
	    
            File f = null;
            try {
		URL object = new URL("http://reporting:5000/pdf?amountRepaid="
		  + request.queryParams("repaid")
		  + "&replacementFixRate=" 
		  + request.queryParams("newRate"));
		HttpURLConnection con = (HttpURLConnection) object.openConnection();
		con.setDoOutput(true);
		con.setDoInput(true);
		con.setRequestProperty("Content-Type", "application/json");
		con.setRequestProperty("Accept", "application/pdf");
		con.setRequestMethod("POST");
		OutputStreamWriter wr = new OutputStreamWriter(con.getOutputStream());
		// The body for the request to reporting is simply the JSON mortgage content that was sent here
		wr.write(request.body());
		wr.flush();

		if (con.getResponseCode() == HttpURLConnection.HTTP_OK) {
	            f = File.createTempFile("prefix",null,new File("/tmp"));
		    InputStream inputStream = con.getInputStream();
		    FileOutputStream outputStream = new FileOutputStream(f);
		    int bytesRead = -1;
		    byte[] buffer = new byte[8192];
		    while ((bytesRead = inputStream.read(buffer)) != -1) {
		        outputStream.write(buffer, 0, bytesRead);
		    }
		    outputStream.close();
		    inputStream.close();
		} else {
		    System.out.println("Error coming from reporting : " + con.getResponseMessage());  
		}  
            } catch (Exception e) {
              System.out.println("Error calling reporting : " + e.toString());
              return e.toString();
            }

	    try {
	            MailSender sender = new MailSender();
	            String result = sender.send(username, password, "Repayment report", request.queryParams("email"), f.getPath());
	            f.delete();
	            return result;
	    } catch (Exception e) {
              System.out.println("Error while sending mail : " + e.toString());
              return e.toString();
	    }
        });

    }
}
