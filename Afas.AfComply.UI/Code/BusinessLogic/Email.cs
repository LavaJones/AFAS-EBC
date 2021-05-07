using Afas.AfComply.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Net.Mail;

/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{

    public Email()
    {
        this.Log = LogManager.GetLogger(typeof(Email));
    }

    /// <summary>
    /// Builds a branding aware HTML footer safe for inclusion on all emails. Placeholder on the way to templated emails vs. coded emails.
    /// </summary>
    /// <returns></returns>
    public string BuildEmailFooter()
    {

        string footer = null;

        footer += string.Format("<h3 style='color:blue'>{0}</h3>", Branding.CompanyName);
        footer += string.Format("{0}<br />", Branding.AddressStreet);
        footer += string.Format("{0}<br />", Branding.AddressCityState);
        footer += string.Format("Phone: {0}", Branding.PhoneNumber);

        return footer;

    }

    /// <summary>
    /// Builds a standard MailMessage with the passed parameters. Placeholder on the way to templated emails vs. coded emails.
    /// </summary>
    public MailMessage FormatEmailMessage(string subject, string body)
    {

        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(SystemSettings.EmailNotificationAddress),              
            Subject = Branding.ProductName + " - " + subject,
            Body = body + this.BuildEmailFooter(),         
            IsBodyHtml = true
        };

        return mailMessage;

    }

    public bool SendEmail(string e_to, string subject, string body, bool _cc)
    {

        this.Log.Info(string.Format("Sending email to {0}, subject {1}", e_to, subject));

        try
        {

            SmtpClient smtpClient = new SmtpClient
            {
                EnableSsl = Feature.EnableSsl
            };

            MailMessage mailMessage = this.FormatEmailMessage(subject, body);

            this.SetTo(mailMessage, e_to);

            if (_cc == true)
            {
                this.SetCC(mailMessage, SystemSettings.EmailNotificationAddress);
            }

            smtpClient.Send(mailMessage);

            return true;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Error Sending Email: ", exception);

            return false;

        }

    }

    public bool sendEmailInvoice(List<User> _users, string subject, string body, bool _cc)
    {
        try
        {

            SmtpClient smtpClient = new SmtpClient
            {
                EnableSsl = Feature.EnableSsl
            };

            MailMessage mailMessage = this.FormatEmailMessage(subject, body);

            foreach (User u in _users)
            {
                this.SetTo(mailMessage, u.User_Email);
            }

            if (_cc == true)
            {
                this.SetCC(mailMessage, SystemSettings.BillingEmailAddress);
            }

            smtpClient.Send(mailMessage);

            return true;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return false;

        }

    }

    public bool sendEmail(List<User> _users, string subject, string body, bool _cc)
    {
        try
        {

            SmtpClient smtpClient = new SmtpClient
            {
                EnableSsl = Feature.EnableSsl
            };

            MailMessage mailMessage = this.FormatEmailMessage(subject, body);

            foreach (User u in _users)
            {
                this.SetTo(mailMessage, u.User_Email);
            }

            if (_cc == true)
            {
                this.SetCC(mailMessage, SystemSettings.EmailNotificationAddress);
            }

            smtpClient.Send(mailMessage);

            return true;
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return false;

        }

    }

    /// <summary>
    /// Sets the To section of the email message honoring the Feature.UseUsersEmailAddress setting.
    /// </summary>
    public void SetCC(MailMessage mailMessage, string ccEmailAddress)
    {

        string emailAddress = SystemSettings.EmailOverrideAddress;

        if (Feature.IsRealEmailsEnabled)
        {
            emailAddress = ccEmailAddress;
        }

        mailMessage.CC.Add(new MailAddress(emailAddress));

    }

    /// <summary>
    /// Sets the To section of the email message honoring the Feature.UseUsersEmailAddress setting.
    /// </summary>
    public void SetTo(MailMessage mailMessage, string toEmailAddress)
    {

        string emailAddress = SystemSettings.EmailOverrideAddress;

        if (Feature.IsRealEmailsEnabled)
        {
            emailAddress = toEmailAddress;
        }

        mailMessage.To.Add(new MailAddress(emailAddress));

    }

    protected ILog Log { get; private set; }

}