using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Parsers
{
    public class CroneVerifier : ICroneVerifier
    {
        private class Boundary
        {
            public int Lower { get; set; }
            public int Upper { get; set; }
            public Boundary(int lower, int upper)
            {
                Lower = lower;
                Upper = upper;
            }
            public bool ValueWithinBoundary(int value)
            {
                return value >= Lower && value <= Upper;
            }
        }
        private class CroneVerificationException : Exception
        {
        }
        private readonly List<char> AllowedSpecialChars = new List<char>
        {
            '*', ',','-'
        };
        private List<Boundary> boundaries = new List<Boundary>
        {
            new Boundary(0, 59),
            new Boundary(0, 23),
            new Boundary(1,31),
            new Boundary(1,12),
            new Boundary(0,6)
        };
        private Boundary GetBoundarieForEntrie(int entrieNumber)
        {
            return boundaries[entrieNumber];
        }
        private int Cron_Entries_Number = 5;
        public bool VerifyCron(string cron)
        {
            string[] entries = cron.Split(' ');

            int EntriesNumber = entries.Count();
            if (EntriesNumber != Cron_Entries_Number)
            {
                return false;
            }

            for (int i = 0; i < entries.Length; i++)
            {
                try
                {
                    VerifyEntrie(entries[i], i);
                }
                catch (CroneVerificationException)
                {
                    return false;
                }
            }
            return true;
        }

        private void VerifyEntrie(string entrie, int entriePosition)
        {
            Boundary boundary = GetBoundarieForEntrie(entriePosition);
            for (int i = 0; i < entrie.Length; i++)
            {
                char character = entrie[i];
                if (IsDigit(character))
                {
                    int number = GetIntRepresentation(character);
                    int nextCharacterIndex = i + 1;
                    if (nextCharacterIndex < entrie.Length)
                    {
                        char nextCharacter = entrie[nextCharacterIndex];
                        if (IsDigit(nextCharacter))
                        {
                            number *= 10;
                            number += GetIntRepresentation(nextCharacter);
                            i++;
                        }
                    }
                    bool IsNotWithinBoundarie = !boundary.ValueWithinBoundary(number);
                    if (IsNotWithinBoundarie)
                    {
                        throw new CroneVerificationException();
                    }
                }
                else
                {
                    bool IsNotSpecialCharacter = !AllowedSpecialChars.Contains(character);
                    if (IsNotSpecialCharacter)
                    {
                        throw new CroneVerificationException();
                    }
                    else
                    {
                        if (character == '-')
                        {
                            if (i == 0)
                            {
                                throw new CroneVerificationException();
                            }
                        }
                    }
                }
            }
        }
        private bool IsDigit(char character)
        {
            return Char.IsDigit(character);
        }
        private int GetIntRepresentation(char character)
        {
            return character - '0';
        }
    }

}
