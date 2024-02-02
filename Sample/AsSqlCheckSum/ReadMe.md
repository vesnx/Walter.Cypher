## Using `AsSqlChecksum` for Efficient Database Indexing

This example demonstrates how to use the `AsSqlChecksum` extension method to insert and retrieve user information from a database, leveraging the efficiency of integer checksums for the email column.

### Prerequisites

Make sure you have the installed the `Walter.Cypher` nuget package.


### Inserting User Data

When inserting data into the `users` table, calculate the checksum for the email and store both the email and its checksum:
```csharp
string userEmail = "user@example.com";
int emailChecksum = userEmail.AsSqlChecksum();

using (var connection = new SqlConnection("YourConnectionString"))
{
    string sql = "INSERT INTO users (email, usercs, other_columns) VALUES (@Email, @UserCs, @OtherValues)";

    using (var command = new SqlCommand(sql, connection))
    {
        command.Parameters.AddWithValue("@Email", userEmail);
        command.Parameters.AddWithValue("@UserCs", emailChecksum);
        // Add other parameters as necessary

        connection.Open();
        command.ExecuteNonQuery();
    }
}
### Retrieving User Data

To retrieve user information, first search by the checksum (`usercs`) and email to quickly narrow down the results using efficient integer indexing:

```csharp

string searchEmail = "user@example.com";
int searchChecksum = searchEmail.AsSqlChecksum();

using (var connection = new SqlConnection("YourConnectionString"))
{
    string sql = "SELECT * FROM users WHERE usercs = @UserCs AND email = @Email";

    using (var command = new SqlCommand(sql, connection))
    {
        command.Parameters.AddWithValue("@Email", searchEmail);
        command.Parameters.AddWithValue("@UserCs", searchChecksum);

        connection.Open();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                // Process each matching user
            }
        }
    }
}
```
### The Index
To fully leverage the performance benefits of using a checksum for querying data, it's essential to create an efficient index on the database. This index should be based on the checksum column, with the email column included for quick access during query operations. You can create such an index using the following T-SQL statement:
```TSQL
CREATE NONCLUSTERED INDEX [IX_users_Checksum] ON [dbo].[users]
(
	[Checksum] ASC
)
INCLUDE([Email]) 
```
This statement creates a nonclustered index on the `users` table, specifically targeting the `usercs` (checksum) column. The index is sorted in ascending order (`ASC`). Including the `Email` column in the index allows SQL Server to directly access email values from the index without needing to look up the full table row, enhancing query performance when filtering by both checksum and email.

By structuring your index in this manner, you ensure that queries which filter by the checksum and then verify against the exact email address can be executed with optimal efficiency. This is particularly useful in scenarios where the checksum column is used to quickly narrow down search results, and the email column is then used to precisely identify the correct record.

### Usage Note

When implementing this index, ensure that your database schema matches the column names used in the index creation script. In the example provided, `[usercs]` should be the column storing the integer checksum values, and `[Email]` should store the user's email addresses. Adjust the column names as necessary to fit your schema.

### Note

This approach optimizes query performance by utilizing an integer checksum index, ensuring rapid retrieval of user data with minimal impact from potential string comparison overhead.

Make sure to replace `"YourConnectionString"` with your actual database connection string and adjust the SQL commands as needed to match your table schema and application logic. This example provides a straightforward way to integrate the `AsSqlChecksum` method into database operations for enhanced performance and scalability.
