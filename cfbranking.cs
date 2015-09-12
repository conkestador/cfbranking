using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.IO;

namespace cfbranking
{
    static class cfbranking
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            /*InternetScraper scraper = new InternetScraper();
            scraper.setURL("http://sports.yahoo.com/ncaa/football/stats/byteam?cat1=offense&cat2=Total&conference=I-A_SEC");
            scraper.grabStats();
            scraper.readHTML();*/

            List<Team> teams = new List<Team>();
            Team team;

            // read in the cfbstats.csv file
            try
            {
                var reader = new StreamReader(File.OpenRead(@"..\..\cfbstats.csv"));
                
                // read the header line
                reader.ReadLine();
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var stats = line.Split(',');

                    team = new Team(stats);
                    teams.Add(team);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("A file not found error occurred: {0}", e);
            }
            catch (IOException e)
            {
                Console.WriteLine("An IO exception occurred {0}", e);
            }

            foreach (Team aTeam in teams)
                Console.WriteLine(aTeam.teamName);

            //teams.Sort(delegate(Team x, Team y)
            //{
            //    /*if (x.pointsPerGame == null && y.pointsPerGame == null) return 0;
            //    else if (x.PartName == null) return -1;
            //    else if (y.PartName == null) return 1;
            //    else return x.PartName.CompareTo(y.PartName);*/

            //    if (x.pointsPerGame == y.pointsPerGame)
            //        return 0;
            //    else if (x.pointsPerGame < y.PointsPerGame)
            //        return -1;
            //    else if (x.pointsPerGame > y.pointsPerGame)
            //        return 1;
            //});

            teams.Sort((x, y) => x.pointsPerGame.CompareTo(y.pointsPerGame));

            int teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.pointsPerGame, teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.pointsPerGameOpp.CompareTo(y.pointsPerGameOpp));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += 129 - teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.pointsPerGameOpp, 129 - teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.yardsPerPlay.CompareTo(y.yardsPerPlay));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.yardsPerPlay, teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.yardsPerPlayOpp.CompareTo(y.yardsPerPlayOpp));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += 129 - teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.yardsPerPlayOpp, 129 - teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.redZoneEfficiency.CompareTo(y.redZoneEfficiency));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.redZoneEfficiency, teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.redZoneEfficiencyOpp.CompareTo(y.redZoneEfficiencyOpp));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamScore += 129 - teamRanking;
                Console.WriteLine("{0}, {1}, {2}, {3}", aTeam.teamName, aTeam.redZoneEfficiencyOpp, 129 - teamRanking, aTeam.teamScore);
                teamRanking++;
            }

            teams.Sort((x, y) => x.teamScore.CompareTo(y.teamScore));

            teamRanking = 1;
            foreach (Team aTeam in teams)
            {
                aTeam.teamRanking = 129 - teamRanking;
                Console.WriteLine("{0}, {1}, {2}", aTeam.teamName, aTeam.teamScore, 129 - teamRanking);
                teamRanking++;
            }
        }
    }

    class Team : IEquatable<Team>, IComparable<Team>
    {
        public String teamName;
        public String record;
        public int gamesWon, gamesLost;
        public Double pointsPerGame, pointsPerGameOpp;
        public Double yardsPerPlay, yardsPerPlayOpp;
        public Double firstDownsPerGame, firstDownsPerGameOpp;
        public Double redZoneEfficiency, redZoneEfficiencyOpp;
        public Double interceptionsPerGame, interceptionsPerGameOpp;
        public Double penaltiesPerGame, penaltiesPerGameOpp;

        // my own stats
        public Double pointsPerYard, pointsPerYardOpp;
        public Double teamScore;
        public int teamRanking;

        public Team(string[] stats)
        {
            teamName = stats[56];
            //record = stats[];
            if (stats[50] == "-")
                pointsPerGame = 0.0;
            else
                pointsPerGame = Convert.ToDouble(stats[50]);
            if (stats[16] == "-")
                pointsPerGameOpp = 100.0;
            else
                pointsPerGameOpp = Convert.ToDouble(stats[16]);

            if (stats[14] == "-")
                yardsPerPlay = 0.0;
            else
                yardsPerPlay = Convert.ToDouble(stats[14]);
            if (stats[3] == "-")
                yardsPerPlayOpp = 100.0;
            else
                yardsPerPlayOpp = Convert.ToDouble(stats[3]);

            if (stats[18] == "-")
                redZoneEfficiency = 0.0;
            else
                redZoneEfficiency = Convert.ToDouble(stats[18].Substring(0, stats[18].Length - 1));
            if (stats[28] == "-")
                redZoneEfficiencyOpp = 100.0;
            else
                redZoneEfficiencyOpp = Convert.ToDouble(stats[28].Substring(0, stats[28].Length - 1));

            teamRanking = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Team objAsTeam = obj as Team;
            if (objAsTeam == null) return false;
            else return Equals(objAsTeam);
        }

        // Default comparer for Team type. 
        public int CompareTo(Team compareTeam)
        {
            // A null value means that this object is greater. 
            if (compareTeam == null)
                return 1;
            else
                return this.teamRanking.CompareTo(compareTeam.teamRanking);
        }

        public override int GetHashCode()
        {
            return teamRanking;
        }

        public bool Equals(Team other)
        {
            if (other == null) return false;
            return (this.teamRanking.Equals(other.teamRanking));
        }
    }

    class InternetScraper
    {
        String url;
        String html;

        public InternetScraper()
        {
            
        }

        public void grabStats()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            webResponse.Close();
        }

        /*public void readHTML()
        {
            // The HtmlWeb class is a utility class to get the HTML over HTTP
            HtmlWeb htmlWeb = new HtmlWeb();

            // Creates an HtmlDocument object from an URL
            HtmlDocument document = htmlWeb.Load(url);

            // Extracts all links under a specific node that have an href that begins with "http://"
            HtmlNodeCollection allLinks = document.DocumentNode.SelectNodes("//*[@id='mynode']//a[starts-with(@href,'http://')]");

            // Outputs the href for external links
            foreach (HtmlNode link in allLinks)
                Console.WriteLine(link.Attributes["href"].Value);
        }*/

        public void setURL(String _url)
        {
            url = _url;
        }
    }
}
