using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;

using log4net;

/// <summary>
/// A static helper class to simplify the logging of Authentication/Authorization events while we work on a more permanent solution
/// </summary>
public static class SecurityLogger
{

    /// <summary>
    /// Record a failed login attempt for a user.
    /// </summary>
    public static void LogFailedLogin(System.Web.HttpRequestBase request, String attemptedUsername)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Login failed for [{0}] from IP [{1}].",
                        attemptedUsername,
                        SecurityLogger.ResolveClientIPAddress(request)
                    )
            );

    }

    /// <summary>
    /// Record a failed login attempt for a user.
    /// </summary>
    public static void LogFailedPasswordAttempt(String attemptedUsername)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Login attempt failed for [{0}] from IP [{1}], username was not found or the username/password combination did not match.",
                        attemptedUsername,
                        "unavailable"
                    )
            );

    }

    /// <summary>
    /// Record a page access outside of a login session.
    /// </summary>
    public static void LogInvalidAccess(System.Web.HttpRequestBase request)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("The secured page [{0}] was accessed by client [{1}] without being loggedin, returning to the login screen.",
                        request.RawUrl,
                        SecurityLogger.ResolveClientIPAddress(request)
                    )
            );

    }

    /// <summary>
    /// Record a page access outside of a login session.
    /// </summary>
    public static void LogInvalidAccessAdmin(System.Web.HttpRequestBase request)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("The admin page [{0}] was accessed by client [{1}] without being loggedin, returning to the login screen.",
                        request.RawUrl,
                        SecurityLogger.ResolveClientIPAddress(request)
                    )
            );

    }

    /// <summary>
    /// Record a new login session has been established for a user.
    /// </summary>
    public static void LogLogin(System.Web.HttpRequestBase request, String username)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Login session established for [{0}] from IP [{1}].",
                        username,
                        SecurityLogger.ResolveClientIPAddress(request)
                    )
            );

    }

    /// <summary>
    /// Record a password reset email was sent.
    /// </summary>
    public static void LogPasswordReset(System.Web.HttpRequestBase request, String username, String emailAddress)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Password reset email for [{0}] from IP [{1}] has been sent to [{2}].",
                        username,
                        SecurityLogger.ResolveClientIPAddress(request),
                        emailAddress
                    )
            );

    }

    /// <summary>
    /// Record a password reset email was attempted but failed.
    /// </summary>
    public static void LogFailedPasswordReset(System.Web.HttpRequestBase request, String username, String emailAddress)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Failed to reset Password for [{0}] from IP [{1}] Email [{2}].",
                        username,
                        SecurityLogger.ResolveClientIPAddress(request),
                        emailAddress
                    )
            );

    }


    /// <summary>
    /// Record a username recovery email has been sent.
    /// </summary>
    public static void LogUsernameRecovery(System.Web.HttpRequestBase request, String username, String emailAddress)
    {

        SecurityLogger.LogSecurityEvent(
                String.Format("Username recovery email for [{0}] from IP [{1}] has been sent to [{2}].",
                        username,
                        SecurityLogger.ResolveClientIPAddress(request),
                        emailAddress
                    )
            );

    }

    /// <summary>
    /// Adds a Log message using the SecurityLogger namespace
    /// </summary>
    internal static void LogSecurityEvent(String message)
    {

        if (log.IsInfoEnabled)
        {
            log.Info(message);
        }
        else
        {
            log.Error(String.Format("Unable to log Security event at the proper logging level: {0}", message));
        }

    }

    /// <summary>
    /// Walk through the request object and determine the client IPs address.
    /// In the case of multiple IPs for proxied/load balanced situations we return them all.
    /// </summary>
    public static String ResolveClientIPAddress(System.Web.HttpRequestBase request)
    {

        if (request.IsLocal)
        {
            return request.UserHostAddress;
        }

        String forwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (String.IsNullOrEmpty(forwardedFor))
        {

            log.Error("Unable to determine client ip address, missing load balancer configuration.");

            return String.Format("loadbalancer-not-configured,{0}", request.UserHostAddress);

        }

        if (forwardedFor.Length > 0)
        {
            return String.Format("{0},{1}", forwardedFor.Trim(), request.UserHostAddress);
        }

        return request.UserHostAddress;

    }


    private static ILog log = LogManager.GetLogger(String.Format("SecurityLogger.{0}", typeof(SecurityLogger).FullName));

}
