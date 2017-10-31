using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.WireProtocol.GetMetadataResponse;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class SubCommandPerformanceTests
    {
        private const int NumIterations = 50000000;
        static List<ICmd> commands = new List<ICmd>();
        private static TestContext _context;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            _context = context;
            // build list 
            commands = new List<ICmd>();
            for (int i = 0; i < NumIterations; i++)
            {
                int subcommand = i % 7;
                switch (subcommand)
                {
                    case 0:
                        commands.Add(new CmdAddColumn());
                        break;
                    case 1:
                        commands.Add(new CmdAddRow());
                        break;
                    case 2:
                        commands.Add(new CmdAddTable());
                        break;
                    case 3:
                        commands.Add(new CmdAddValue());
                        break;
                    case 4:
                        commands.Add(new CmdDatabaseVersion());
                        break;
                    case 5:
                        commands.Add(new CmdClear());
                        break;
                    case 6:
                        commands.Add(new CmdDeleteRow());
                        break;
                }
            }
        }



        [TestMethod]
        public void TestCasting()
        {
            ICmd cmd = new CmdAddColumn();

            for (int i = 0; i < NumIterations; i++)
            {
                CmdAddColumn result = (CmdAddColumn)cmd;
                result.ColumnIndex++;
            }
        }

        [TestMethod]
        public void TestAssignments()
        {
            ICmd cmd = new CmdAddColumn();

            for (int i = 0; i < NumIterations; i++)
            {
                CmdAddColumn result = cmd.AddColumn;
                result.ColumnIndex++;
            }
        }

        [TestMethod]
        public void TestSwitchCommandAssignement()
        {
            for (int i = 0; i < NumIterations; i++)
            {
                ICmd cmd = commands[i];
                switch (cmd.SubCommand)
                {
                    case SubCommand.DatabaseVersion:
                        Assert.IsNotNull(cmd.DatabaseVersion);
                        break;
                    case SubCommand.Clear:
                        Assert.IsNotNull(cmd.Clear);
                        break;
                    case SubCommand.AddTable:
                        Assert.IsNotNull(cmd.AddTable);
                        break;
                    case SubCommand.AddColumn:
                        Assert.IsNotNull(cmd.AddColumn);
                        break;
                    case SubCommand.AddRow:
                        Assert.IsNotNull(cmd.AddRow);
                        break;
                    case SubCommand.AddValue:
                        Assert.IsNotNull(cmd.AddValue);
                        break;
                    case SubCommand.DeleteRow:
                        Assert.IsNotNull(cmd.DeleteRow);
                        break;
                    case SubCommand.Invalid:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [TestMethod]
        public void TestSwitchCommandCast()
        {
            for (int i = 0; i < NumIterations; i++)
            {
                ICmd cmd = commands[i];
                switch (cmd.SubCommand)
                {
                    case SubCommand.DatabaseVersion:
                        var cmdDatabaseVersion = (CmdDatabaseVersion)cmd;
                        Assert.IsNotNull(cmdDatabaseVersion);
                        break;
                    case SubCommand.Clear:
                        var cmdClear = (CmdClear)cmd;
                        Assert.IsNotNull(cmdClear);
                        break;
                    case SubCommand.AddTable:
                        var cmdAddTable = (CmdAddTable)cmd;
                        Assert.IsNotNull(cmdAddTable);
                        break;
                    case SubCommand.AddColumn:
                        var cmdAddColumn = (CmdAddColumn)cmd;
                        Assert.IsNotNull(cmdAddColumn);
                        break;
                    case SubCommand.AddRow:
                        var cmdAddRow = (CmdAddRow)cmd;
                        Assert.IsNotNull(cmdAddRow);
                        break;
                    case SubCommand.AddValue:
                        var cmdAddValue = (CmdAddValue)cmd;
                        Assert.IsNotNull(cmdAddValue);
                        break;
                    case SubCommand.DeleteRow:
                        var cmdDeleteRow = (CmdDeleteRow)cmd;
                        Assert.IsNotNull(cmdDeleteRow);
                        break;
                    case SubCommand.Invalid:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [TestMethod]
        public void TestPatternMatching()
        {
            for (int i = 0; i < NumIterations; i++)
            {
                ICmd cmd = commands[i];

                switch (cmd)
                {
                    case CmdDatabaseVersion cmdDatabaseVersion:
                        Assert.IsNotNull(cmdDatabaseVersion);
                        break;
                    case CmdClear cmdClear:
                        Assert.IsNotNull(cmdClear);
                        break;
                    case CmdAddTable cmdAddTable:
                        Assert.IsNotNull(cmdAddTable);
                        break;
                    case CmdAddColumn cmdAddColumn:
                        Assert.IsNotNull(cmdAddColumn);
                        break;
                    case CmdAddRow cmdAddRow:
                        Assert.IsNotNull(cmdAddRow);
                        break;
                    case CmdAddValue cmdAddValue:
                        Assert.IsNotNull(cmdAddValue);
                        break;
                    case CmdDeleteRow cmdDeleteRow:
                        Assert.IsNotNull(cmdDeleteRow);
                        break;
                }
            }
        }

    }
}
