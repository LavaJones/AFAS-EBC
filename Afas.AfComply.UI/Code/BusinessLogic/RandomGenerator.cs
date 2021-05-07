using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// RandomGenerator is a utility class for generating Random or Psuedo Random Values such as Nonces
/// </summary>
public class RandomGenerator
{

    /// <summary>
    /// Random Generator used in this RandomGenerator to provide a source of Randomness
    /// </summary>
    private Random random = new Random((int)DateTime.Now.Ticks);          

    /// <summary>
    /// Generate a suitable Random Password value
    /// </summary>
    /// <returns></returns>
    public string AutoGeneratePassword()
    {

        int passwordLength = random.Next(15, 20);

        return RandomNonceString(passwordLength);

    }

    /// <summary>
    /// Generates a Random Nonce of the desired lenght
    /// </summary>
    /// <param name="nonceLength">The desired length of the Nonce being generated.</param>
    /// <returns>A string of nonceLength consisting of random values</returns>
    private string RandomNonceString(int nonceLength)
    {

        Guid randGuid1 = Guid.NewGuid();
        Guid randGuid2 = Guid.NewGuid();
        long timeTicks = DateTime.Now.Ticks;
        int rand1 = random.Next();
        int rand2 = random.Next();

        string merged = rand1.ToString() + randGuid1.ToString() + timeTicks.ToString() + randGuid2.ToString() + rand2.ToString();

        string base64String = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(merged));

        int maxSubStrStart = base64String.Length - nonceLength;
        int subStrStart = random.Next(0, maxSubStrStart);
        string subString = base64String.Substring(subStrStart, nonceLength);

        return subString;

    }

}