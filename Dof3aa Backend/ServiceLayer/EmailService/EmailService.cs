using MailKit.Net.Smtp;
using MimeKit;
using System.Net;


namespace ServiceLayer.EmailService

{
    public class EmailService
    {
        public static void SendEmail(MimeMessage message)
        {

            using (var client = new SmtpClient())
            {
                // Connect to the SMTP server
                client.Connect("smtp.gmail.com", 587, false);

                // If your SMTP server requires authentication
                client.Authenticate("", "");

                // Send the email
                client.Send(message);

                // Disconnect from the SMTP server
                client.Disconnect(true);
            }
            

        }
        public static string GetEmailBodyForEmailConfirm(string link)
        {
            var Content = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Confirmation</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n        }}\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n        }}\r\n        h1 {{\r\n            color: #333;\r\n        }}\r\n        p {{\r\n            color: #666;\r\n        }}\r\n        .confirm-link {{\r\n            display: inline-block;\r\n            background-color: #007bff;\r\n            color: #fff;\r\n            text-decoration: none;\r\n            padding: 10px 20px;\r\n            border-radius: 5px;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Confirm Your Email Address</h1>\r\n        <p>Thank you for signing up! Please click the button below to confirm your email address:</p>\r\n        <a href=\"{link}\" class=\"confirm-link\">Confirm Email Address</a>\r\n        <p>If you did not sign up for an account, you can safely ignore this email.</p>\r\n    </div>\r\n</body>\r\n</html>\r\n";
                return Content;
        }

        public static string GetEmailBodyForPasswordReset(string? link)
        {
            var Content = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Password Update Token</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            text-align: center;\r\n        }}\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 100px auto;\r\n            padding: 20px;\r\n            border: 1px solid #ccc;\r\n            border-radius: 5px;\r\n            background-color: #f9f9f9;\r\n        }}\r\n        h1 {{\r\n            color: #333;\r\n        }}\r\n        p {{\r\n            color: #666;\r\n        }}\r\n        .token {{\r\n            font-size: 24px;\r\n            margin-bottom: 20px;\r\n        }}\r\n        .ignore {{\r\n            font-style: italic;\r\n            color: #999;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Password Update Token</h1>\r\n        <p>Use the following token to update your password:</p>\r\n        <div class=\"token\"> {link} </div>\r\n        <p class=\"ignore\">If you didn't request a password update, you can safely ignore this email.</p>\r\n    </div>\r\n</body>\r\n</html>\r\n";
            return Content;
        }
    }
}
