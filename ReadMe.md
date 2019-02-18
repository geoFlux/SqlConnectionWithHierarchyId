Allows one to use SqlHierarchyId in .net core projects.
I made this mainly to work with Dapper. It uses dotMorten.Microsoft.SqlServer.Types.
You need to do SqlConnectionWithHierarchyId.AddAddDapperSupport in order for Dapper to map the type correctly.