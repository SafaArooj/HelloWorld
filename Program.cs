﻿using System;
using Starcounter;

namespace HelloWorld {
    class Program {

        static void Main() {

            //on the first run, create the initial Spender object
            var spender = Db.SQL<Spender>("SELECT s FROM Spender s").First;
            if (spender == null) {
                Db.Transact(() => {
                    new Spender() {
                        Name = "Martin's Expenses List"
                    };
                });
            }

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            //create HTTP handler
            Handle.GET("/HelloWorld", () => {
                return Db.Scope(() => {
                    var json = new ExpensesList() {
                        Data = Db.SQL<Spender>("SELECT s FROM Spender s").First
                    };
                    json.NewExpense.Data = new Expense() {
                        Amount = 1
                    };
                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });
        }
    }
}