﻿using Project.Base;
using Project.Enums;
using Project.Helpers;
using Project.Models;
using Project.Services;
using System;

namespace Project.Commands {

    class ChairDelete : Command {

        public override string GetCategory() {
            return "chair";
        }

        public override string GetName() {
            return "delete";
        }

        public override bool RequireAdmin() {
            return true;
        }

        public override void RunCommand(string[] args) {
            Program app = Program.GetInstance();
            ChairService chairService = app.GetService<ChairService>("chairs");

            // Check args length
            if (args.Length != 1) {
                throw new ArgumentException("Gebruik: chair/delete <id>");
            }

            // Find room
            int id = ConsoleHelper.ParseInt(args[0], "id");
            Chair chair = chairService.GetChairById(id);

            if (chair == null) {
                throw new ArgumentException("Ongeldige stoel");
            }

            if (chairService.DeleteChair(chair)) {
                ConsoleHelper.Print(PrintType.Info, "Stoel succesvol verwijderd");
            } else {
                ConsoleHelper.Print(PrintType.Error, "Kon stoel niet verwijderen");
            }
        }

    }

}
