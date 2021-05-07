using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


/// <summary>
/// Helper class to handle all of the Authroization deployment variables.
/// </summary>
public static class Authorization
{
    /// <summary>
    /// Gets the setting for the number of passses to use when creating a Password Salt
    /// </summary>
    public static int NumberOfSaltPasses
    {
        get
        {
            int passes = 10000;
            if (int.TryParse(ConfigurationManager.AppSettings["Authorization.NumberSaltPasses"], out passes))
            {
                return passes;
            }
            else 
            {
                return 10000;
            }
        }
    }

    /// <summary>
    /// Gets the setting for the number of passses to use when creating a Password hash
    /// </summary>
    public static int NumberOfPasswordPasses
    {
        get
        {
            int passes = 10000;
            if (int.TryParse(ConfigurationManager.AppSettings["Authorization.NumberPasswordPasses"], out passes))
            {
                return passes;
            }
            else
            {
                return 10000;
            }
        }
    }

    /// <summary>
    /// Gets the setting for the min number for pasword length
    /// </summary>
    public static int MinPasswordLength
    {
        get
        {
            int passwordLength = 6;
            if (int.TryParse(ConfigurationManager.AppSettings["Authorization.MinPasswordLength"], out passwordLength))
            {
                return passwordLength;
            }
            else
            {
                return 6;
            }
        }
    }

    /// <summary>
    /// Gets the Base Salt for generating the userspecific Salt
    /// </summary>
    public static string BaseSalt 
    { 
        get 
        { 
            return ConfigurationManager.AppSettings["Authorization.BaseSalt"]; 
        }
    }
}
