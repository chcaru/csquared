using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CSquared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSquaredTests
{
    [TestClass]
    public class LexerKeywordsTests
    {

        private readonly static string ConditionalsContents = ResourceReader("conditionals.csqr");

        private static string ResourceReader(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream("CSquaredTests.TestPrograms." + resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private Lexer CreateLexer(string fileContents)
        {
            var reader = new Reader();

            reader.SetFileContents(fileContents);

            return new Lexer(reader);
        }

        private IEnumerable<ILexeme> LexemeEnumerator(Lexer lexer)
        {
            var lexeme = lexer.Lex();

            do
            {
                yield return lexeme;
                lexeme = lexer.Lex();
            }
            while (lexeme.Type != LexemeType.EndOfInput);
        }

        /// <summary>
        /// Asserts that each non-null LexemeType provided to types is present, in order, in the lexical stream as output from content.
        /// </summary>
        /// <param name="content">The content which is input to the Lexer</param>
        /// <param name="types">The LexemeTypes expected to be present in the lexical output stream. Null types are interpreted as place holders for which their corresponding lexical output is not relevant (ignored)</param>
        private void LexemeTester(IEnumerable<ILexeme> lexemes, params LexemeType?[] types)
        {
            var lexemeTypePairs = 
                lexemes.Zip(
                    types, 
                    (l, lt) => new Tuple<ILexeme, LexemeType?>(l, lt)
                );

            foreach (var pair in lexemeTypePairs)
            {
                if (pair.Item2 != null)
                {
                    Assert.AreEqual(pair.Item1.Type, pair.Item2);
                }
            }
        }

        [TestMethod]
        public void TestConditionalsKeywords()
        {
            var lexer = this.CreateLexer(ConditionalsContents);

            var lexemes = this.LexemeEnumerator(lexer).ToArray();

            this.LexemeTester(lexemes.Skip(17), LexemeType.If);

            this.LexemeTester(lexemes.Skip(30), LexemeType.Else);
        }
    }
}
