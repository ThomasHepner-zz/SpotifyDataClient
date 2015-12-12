using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.DAL
{
    public class SpotifyIntializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SpotifyContext>
    {
    }
}