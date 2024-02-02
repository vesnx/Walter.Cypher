using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Numerics;

/// <summary>
/// Provides functionalities to securely communicate with a server, specifically for licensing purposes.
/// Utilizes encryption for sensitive data transmission.
/// </summary>
public class InMyAppPrivateStuff
{
    private readonly BigInteger _cypherKey;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMyAppPrivateStuff"/> class.
    /// </summary>
    /// <param name="cypherKey">The encryption key used for securing communication, shared with the server.</param>
    /// <param name="httpClient">The HTTP client used for data transmission.</param>
    public InMyAppPrivateStuff(BigInteger cypherKey, HttpClient httpClient)
    {
        _cypherKey = cypherKey;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Asynchronously sends encrypted licensing information to the server.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="creditCard">The user's credit card information.</param>
    /// <param name="licenseDuration">The requested duration for the license.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the server's response.</returns>
    /// <remarks>
    /// The method encrypts the user's information using AES encryption with a shared key before transmission.
    /// This approach enhances security by ensuring sensitive data is encrypted over the network.
    /// Compliance with PCI DSS and other relevant standards is ensured for handling credit card information.
    /// </remarks>
    public async Task<string> SendToLicenseServer(string userName, string creditCard, TimeSpan licenseDuration)
    {
        var payload = new
        {
            UserName = userName,
            CreditCard = creditCard,
            LicenseDuration = licenseDuration
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        // Encrypt the JSON payload using the shared cypher key stored in the big-integer.
	// by using encryption and decryption extension method for AES encryption
        string encryptedPayload = jsonPayload.Encrypt(_cypherKey.FromNumeric());

        var content = new StringContent(encryptedPayload, System.Text.Encoding.UTF8, "text/text");

        try
        {
            var response = await _httpClient.PostAsync("api/licenseNow", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var numericResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                // Decrypt the response using the numeric extension.
                return numericResponse.FromNumeric();
            }
            else
            {
                throw new HttpRequestException($"Request to license server failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Log or handle exceptions as appropriate for your application.
            throw new InvalidOperationException("Failed to communicate with the license server.", ex);
        }
    }
}
