The cypher package for hashing data in a convenient and secure way as well as symmetric and asymmetric cross platform encryption methods.
You can find  On-line documentation at https://cypherapi.asp-waf.com/ as well as using the sample code found at https://github.com/ASP-WAF/Cypher


# ![Cypher ICON](https://cdn.asp-waf.com/img/Cypher.png) Walter.Cypher
This repository shows how you can use the [Walter.Cypher NuGet package](https://www.nuget.org/packages/Walter.Cypher/) in your code using little code snippets.

## Walter.Web.CypherTests sample code
This sample code shows the use of 
1. Fixed [cypher](https://github.com/ASP-WAF/Cypher/blob/master/Src/SamplesUsingMsTest/CryptoTests.cs) using a password to protect your code.
2. Generating [checksum](https://github.com/ASP-WAF/Cypher/blob/master/Src/SamplesUsingMsTest/ChecksumTest.cs) for tamper detection.
3. [PGP](https://github.com/ASP-WAF/Cypher/blob/master/Src/SamplesUsingMsTest/PGPManagedTests.cs) using various key strengths to protect data
4. The use of [Numeric encryption](https://github.com/ASP-WAF/Cypher/blob/master/Src/SamplesUsingMsTest/StringExtensionsTests.cs) that can be used to defeat base64 scanning tools to detect cyphered constants for password probing as values can be stored in int64 type values


## Get Started


Show how to cipher large amounts of text using the Crypto class
````C#

[TestMethod()]
public void CipherZip()
{
    var sb = new StringBuilder();
    for (var i = 0; i < 1024; i++)
    {
        sb.Append(DateTime.Now.ToString());
    }
    var test = sb.ToString();

    var cypkered = Crypto.Zip(test);
    var expect = Crypto.UnZip(cypkered);
    Assert.AreEqual(test, expect);
}

````

Alternatively you can use the extension method

````C#
[TestMethod()]
public void CypherExtesnionTest()
{
    var testpw = "65654616540546546";
    var cypher = Environment.MachineName.Encrypt(testpw);
    var clear = cypher.Decrypt(testpw);

    Assert.AreEqual(Environment.MachineName, clear);
}
````

The extension method for cypher also allow encryption using public/private key encryption using certificates for text of any length
````c#
[TestMethod]
public void TestSmallStringAsBytes()
{
    using X509Certificate2 encryptCertificate = GetCert(".cer");
    using X509Certificate2 decryptCertificate = GetCert(".pfx", "01234456");

    var certCypher = Environment.MachineName.AsEncryptedBytes(encryptCertificate);
    var clearBytes = certCypher.AsDecryptFromBytes(decryptCertificate);

    Assert.AreEqual(Environment.MachineName, UTF8Encoding.UTF8.GetString(clearBytes));

    // helper method load embedded test certificate resource from assembly
    static X509Certificate2 GetCert(string extension, string password = null)
    {
        var asam = Assembly.GetExecutingAssembly();
        using (var memory = new MemoryStream())
        using (var stream = asam.GetManifestResourceStream(asam.GetManifestResourceNames().First(f => f.EndsWith(extension))))
        {
            stream.CopyTo(memory);
            return new X509Certificate2(memory.ToArray(), password);
        }
    }
}

[TestMethod]
public void TestLargeStringAsBytes()
{
    using X509Certificate2 encryptCertificate = GetCert(".cer");
    using X509Certificate2 cecryptCertificate = GetCert(".pfx","01234456");

    var sb = new StringBuilder();
    for (int i = 0; i < 100; i++)
    {
        sb.Append(Guid.NewGuid());
    }
    var text = sb.ToString();

    var certCypher = text.AsEncryptedBytes(encryptCertificate);
    Assert.AreNotEqual(certCypher.Length, text.Length);

    var clearBytes = certCypher.AsDecryptFromBytes(cecryptCertificate);
    Assert.AreEqual(text, UTF8Encoding.UTF8.GetString(clearBytes));

    X509Certificate2 GetCert(string extension, string password = null)
    {
        var asam = Assembly.GetExecutingAssembly();
        using (var memory = new MemoryStream())
        using (var stream = asam.GetManifestResourceStream(asam.GetManifestResourceNames().First(f => f.EndsWith(extension))))
        {
            stream!.CopyTo(memory);
            return new X509Certificate2(memory.ToArray(), password);
        }
    }
}


````

You also have the possibility to encrypt and decrypt persisted text based on the **hosting machine**, **user executing the application**, **the application** or **process name** 
this feature works on all platforms that support where that support the concept of users, processes and machine names in .NET and is ideal for storing secure data in memory that should not be possible to access 
when creating an application dump file but should survive a reboot.

There are some limitations on some IOT devices that might prevent you from using these features
````C#
public void RoundTrip_EncryptionScope_Process()
{
    var sb = new StringBuilder();
    for (int i = 0; i < 100; i++)
    {
        sb.Append(Guid.NewGuid());
    }
    var text = sb.ToString();

    var certCypher = text.AsEncryptedBytes(scope: EncryptionScope.Process);
    Assert.AreNotEqual(certCypher.Length, text.Length);

    var clearText = certCypher.AsDecryptFromBytes(scope: EncryptionScope.Process);
    Assert.AreEqual(text, clearText);

}

[TestMethod()]
public void RoundTrip_EncryptionScope_User()
{
    var sb = new StringBuilder();
    for (int i = 0; i < 100; i++)
    {
        sb.Append(Guid.NewGuid());
    }
    var text = sb.ToString();

    var certCypher = text.AsEncryptedBytes(scope: EncryptionScope.User);
    Assert.AreNotEqual(certCypher.Length, text.Length);

    var clearText = certCypher.AsDecryptFromBytes(scope: EncryptionScope.User);
    Assert.AreEqual(text, clearText);
}

[TestMethod()]
public void RoundTrip_EncryptionScope_Machine()
{
    var sb = new StringBuilder();
    for (int i = 0; i < 100; i++)
    {
        sb.Append(Guid.NewGuid());
    }
    var text = sb.ToString();

    var certCypher = text.AsEncryptedBytes(scope: EncryptionScope.Machine);
    Assert.AreNotEqual(certCypher.Length, text.Length);

    var clearText = certCypher.AsDecryptFromBytes(scope: EncryptionScope.Machine);
    Assert.AreEqual(text, clearText);
}

````
