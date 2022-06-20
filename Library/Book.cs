using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    public class Book
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; }
        public string Availability { get; set; }
        public DateTime ReturnDate { get; set; }

        public string FullInfo
        {
            get
            {
                return $"BookID: {BookID} | '{ Name }' by { Author }, category: { Category }, language: {Language}\n ISBN: {ISBN}, publication date: {PublicationDate.Year}-{PublicationDate.Month}-{PublicationDate.Day}".ToUpper();
            }
        }

    }
}
