# Numeric Encoding for Passwords

## Introduction

This library introduces a novel approach to encoding and decoding relatively short strings, such as passwords, into numeric representations. The core idea leverages the `AsNumeric()` and `FromNumeric()` extension methods to transform strings into their numeric encoded forms and vice versa. This method provides a lightweight layer of obfuscation, making it more challenging for unauthorized parties to discern the original string values, especially when they are stored or transmitted.

## Why Numeric Encoding?

Numeric encoding offers several benefits for the security-conscious developer:

- **Obfuscation:** Converts strings into a less recognizable numeric format, providing a basic level of security through obscurity.
- **Storage Efficiency:** Numeric representations, particularly for short strings, can be more storage-efficient, depending on the use case.
- **Compatibility:** Encoded numeric values can be easily stored and transmitted across systems that may have limitations on string data types.

## Usage

### Encoding a String

To encode a string into its numeric representation, use the `AsNumeric()` extension method:

```csharp
string password = "Pa$$word";
string numericPassword = password.AsNumeric();
Console.WriteLine(numericPassword); // Outputs the numeric representation of "Pa$$word"
```

### Decoding a Numeric String
To decode a previously encoded numeric string back to its original text, use the FromNumeric() extension method:

```csharp
string numericPassword = "1234567890"; // Example numeric representation
string originalPassword = numericPassword.FromNumeric();
Console.WriteLine(originalPassword); // Outputs the decoded string, e.g., "Pa$$word"
```

### Integration Example

Here's a more detailed example, showcasing how to integrate these methods into a class for secure communication:
```csharp
public class SecureCommunicator
{
    private BigInteger _encryptionKey;

    public SecureCommunicator(BigInteger encryptionKey)
    {
        _encryptionKey = encryptionKey;
    }

    public string EncryptAndSend(string message)
    {
        // Encrypt the message
        string encryptedMessage = message.Encrypt(_encryptionKey.FromNumeric());

        // Send the encrypted message
        // Example: SendOverNetwork(encryptedMessage);

        return encryptedMessage;
    }

    // Assuming a method for network transmission, etc.
}
```
The sample file [InMyAppPrivateStuff](https://github.com/vesnx/Walter.Cypher/edit/main/Sample/InMyAppPrivateStuff.cs) showcases using a BigInteger to facilitate a shared password between an app and a server accepting data via RestApi.  

### Security Considerations
While the `AsNumeric()` and `FromNumeric()` methods add a layer of security by obfuscating string values, they should not be considered a replacement for robust encryption standards like AES for sensitive data protection. Instead, these methods can be part of a broader security strategy, especially for non-critical applications where simplicity and speed are desired.

### Conclusion
The numeric encoding approach provided by `AsNumeric()` and `FromNumeric()` offers a simple and efficient way to handle strings like passwords, making them less apparent and slightly more secure. However, it's important to complement this technique with other security measures for applications requiring higher levels of data protection.](https://github.com/vesnx/Walter.Cypher/edit/main/Sample/InMyAppPrivateStuff.cs] showcases using a BigInteger to facilitate a shared password between an app and a server accepting data via RestApi.  

### Security Considerations
While the `AsNumeric()` and `FromNumeric()` methods add a layer of security by obfuscating string values, they should not be considered a replacement for robust encryption standards like AES for sensitive data protection. Instead, these methods can be part of a broader security strategy, especially for non-critical applications where simplicity and speed are desired.

### Conclusion
The numeric encoding approach provided by `AsNumeric()` and `FromNumeric()` offers a simple and efficient way to handle strings like passwords, making them less apparent and slightly more secure. However, it's important to complement this technique with other security measures for applications requiring higher levels of data protection.