using System;
using System.Collections.Generic;
using System.Text;

namespace CongressCompanion.ClassObjects
{
    public class Rootobject
    {
        public string kind { get; set; }
        public Normalizedinput normalizedInput { get; set; }
        public Divisions divisions { get; set; }
        public Office[] offices { get; set; }
        public Official[] officials { get; set; }
    }

    public class Normalizedinput
    {
        public string line1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class Divisions
    {
        public OcdDivisionCountryUs ocddivisioncountryus { get; set; }
        public OcdDivisionCountryUsStateCa ocddivisioncountryusstateca { get; set; }
    }

    public class OcdDivisionCountryUs
    {
        public string name { get; set; }
        public int[] officeIndices { get; set; }
    }

    public class OcdDivisionCountryUsStateCa
    {
        public string name { get; set; }
        public int[] officeIndices { get; set; }
    }

    public class Office
    {
        public string name { get; set; }
        public string divisionId { get; set; }
        public string[] levels { get; set; }
        public string[] roles { get; set; }
        public int[] officialIndices { get; set; }
    }

    public class Official
    {
        public string name { get; set; }
        public Address[] address { get; set; }
        public string party { get; set; }
        public string[] phones { get; set; }
        public string[] urls { get; set; }
        public string photoUrl { get; set; }
        public Channel[] channels { get; set; }
        public string[] emails { get; set; }
    }

    public class Address
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class Channel
    {
        public string type { get; set; }
        public string id { get; set; }
    }
}