using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            public bool ValueNotWithinBoundary(int value)
            {
                return value < Lower || value > Upper;
            }
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

            int leftSideOfRange = -1;

            for (int i = 0; i < entrie.Length; i++)
            {
                char character = entrie[i];
                if (IsDigit(character))
                {
                    int number = GetIntRepresentation(character);
                    if (NextCharacterIsDigit(i, entrie))
                    {
                        char nextCharacter = GetNextCharacter(i, entrie);
                        number *= 10;
                        number += GetIntRepresentation(nextCharacter);
                        i++;
                    }
                    if (leftSideOfRange != -1)
                    {
                        if (number < leftSideOfRange)
                        {
                            throw new CroneVerificationException();
                        }
                    }
                    if (HasNextCharacter(i, entrie))
                    {
                        char next = GetNextCharacter(i, entrie);
                        if (next == '-')
                        {
                            leftSideOfRange = number;
                        }
                    }
                    bool IsNotWithinBoundarie = boundary.ValueNotWithinBoundary(number);
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
                            ThrowIfRepresentsnegativeNumber(entrie, i);
                        }
                    }
                }
            }
        }
        private void ThrowIfRepresentsnegativeNumber(string entrie, int position)
        {
            if (position == 0)
            {
                throw new CroneVerificationException();
            }
            else
            {
                int previousCharacterIndex = position - 1;
                char previousCharacter = entrie[previousCharacterIndex];
                if (previousCharacter == '-' || previousCharacter == ',')
                {
                    throw new CroneVerificationException();
                }
            }
        }
        private bool NextCharacterIsDigit(int i, string entrie)
        {
            if (HasNextCharacter(i, entrie))
            {
                char nextCharacter = GetNextCharacter(i, entrie);
                return IsDigit(nextCharacter);
            }
            return false;
        }
        private bool HasNextCharacter(int i, string str)
        {
            int nextIndex = i + 1;
            return nextIndex < str.Length;
        }
        private char GetNextCharacter(int i, string str)
        {
            int nextCharacterIndex = i + 1;
            return str[nextCharacterIndex];
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
