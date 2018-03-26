using System;
using System.Collections.Generic;
using Sttp.Codec;
using Sttp.Codec.Metadata;
using Sttp.Core;
using Sttp.Data;

namespace Sttp.Services
{
    public class SttpMetadataClient : ISttpCommandHandler
    {
        private CommandMetadataSchema m_currentSchema;
        private MetadataQueryTable m_workingTable;

        public SttpMetadataClient()
        {


        }

        public void ProcessCommand(CommandMetadata command, WireEncoder encoder)
        {
            MetadataSubCommandObjects subCmd;
            while ((subCmd = command.NextCommand()) != null)
            {
                switch (subCmd.SubCommand)
                {
                    case MetadataSubCommand.DefineResponse:
                        m_workingTable = new MetadataQueryTable(subCmd.DefineResponse);
                        break;
                    case MetadataSubCommand.DefineRow:
                        m_workingTable.ProcessCommand(subCmd.DefineRow);
                        break;
                    case MetadataSubCommand.UndefineRow:
                        m_workingTable.ProcessCommand(subCmd.UndefineRow);
                        break;
                    case MetadataSubCommand.Finished:
                        //Notify the user that a new table result has been received.
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ProcessCommand(CommandMetadataSchema command, WireEncoder encoder)
        {
            m_currentSchema = command;

        }

        public void ProcessCommand(CommandMetadataSchemaUpdate command, WireEncoder encoder)
        {
            m_currentSchema = m_currentSchema?.Combine(command);
        }

        public void ProcessCommand(CommandMetadataSchemaVersion command, WireEncoder encoder)
        {
            //Do nothing, this just means that the schema is not compatible.
        }

        public void ProcessCommand(CommandMetadataVersionNotCompatible command, WireEncoder encoder)
        {
            //update the schema and redo the last command.
        }

        public List<string> CommandsHandled()
        {
            var lst = new List<string>();
            lst.Add("Metadata");
            lst.Add("MetadataSchema");
            lst.Add("MetadataSchemaUpdate");
            lst.Add("MetadataSchemaVersion");
            lst.Add("MetadataVersionNotCompatible");
            return lst;
        }

        public void HandleCommand(CommandObjects command, WireEncoder encoder)
        {
            switch (command.CommandName)
            {
                case "Metadata":
                    ProcessCommand(command.Metadata, encoder);
                    return;
                case "MetadataSchema":
                    ProcessCommand(command.MetadataSchema, encoder);
                    return;
                case "MetadataSchemaUpdate":
                    ProcessCommand(command.MetadataSchemaUpdate, encoder);
                    return;
                case "MetadataSchemaVersion":
                    ProcessCommand(command.MetadataSchemaVersion, encoder);
                    return;
                case "MetadataVersionNotCompatible":
                    ProcessCommand(command.MetadataVersionNotCompatible, encoder);
                    return;
                default:
                    throw new Exception("This command is not supported");
            }
        }
    }
}
