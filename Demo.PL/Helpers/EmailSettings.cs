using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{

			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("passantmohamed127@gmail.com", "frhf nlbc jslm judc");
			client.Send("passantmohamed127@gmail.com", email.Recipients, email.Subject, email.Body);

		}
	}
}
