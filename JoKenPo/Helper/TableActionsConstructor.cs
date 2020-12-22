using System.Collections.Generic;
using System.Collections.ObjectModel;
using JoKenPo.Auxiliar;
using Microsoft.AspNetCore.Components;

namespace JoKenPo.Helper
{
    public class TableActionsConstructor<T>
    {
        private List<TableAction<T>> Actions { get; }

        private TableActionsConstructor()
        {
            Actions = new List<TableAction<T>>();
        }

        public static TableActionsConstructor<T> Initialize()
        {
            var construct = new TableActionsConstructor<T>();
            return construct;
        }

        public TableActionsConstructor<T> AddIconAction(string icone, EventCallback<T> acao)
        {
            Actions.Add(new TableAction<T> {Icon = icone, Action = acao});
            return this;
        }

        public TableActionsConstructor<T> AddTextAction(string texto, EventCallback<T> acao)
        {
            Actions.Add(new TableAction<T> {Text = texto, Action = acao});
            return this;
        }

        public IEnumerable<TableAction<T>> Generate()
        {
            return new ReadOnlyCollection<TableAction<T>>(Actions);
        }
    }
}