using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSquared
{
    public class Reader
    {
        private Stack<char> FileContents { get; set; }
        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }
        public bool EndOfFile
        {
            get { return this.FileContents.Count <= 0; }
        }

        public Reader(string filePath) 
            : this()
        {
            var fileText = File.ReadAllText(filePath);

            this.SetFileContents(fileText);
        }

        public Reader()
        {
            this.LineNumber = 1;
        }

        public void SetFileContents(string fileContents)
        {
            this.FileContents = new Stack<char>(fileContents.Reverse());
        }

        public void PushBack(char? c)
        {
            if (c != null)
            {
                this.FileContents.Push((char)c);
            }
        }

        public char? GetNextChar(bool skipWhitespace = true)
        {
            if (skipWhitespace)
            {
                this.SkipWhitespace();
            }

            if (this.EndOfFile)
            {
                return null;
            }

            this.LinePosition++;
            
            return this.FileContents.Pop();
        }

        public void SkipWhitespace()
        {
            if (this.EndOfFile)
            {
                return;
            }

            var peek = this.FileContents.Peek();

            while (!this.EndOfFile && char.IsWhiteSpace(peek))
            {
                if (peek == '\n')
                {
                    this.LineNumber++;
                    this.LinePosition = 0;
                }
                else
                {
                    this.LinePosition++;
                }

                this.FileContents.Pop();

                if (this.EndOfFile)
                {
                    break;
                }

                peek = this.FileContents.Peek();
            }
        }
    }
}
