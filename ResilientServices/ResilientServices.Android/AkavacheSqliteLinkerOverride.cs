using System;
using Akavache.Sqlite3;
using Xamarin.Forms.Internals;

// Note: This class file is *required* for iOS to work correctly, and is 
// also a good idea for Android if you enable "Link All Assemblies".
namespace ResilientServices.Droid
{
    [Preserve(AllMembers = true)]
    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
        }
    }
}
