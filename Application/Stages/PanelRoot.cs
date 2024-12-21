using System;
using Kiro.Domain;
using Kiro.Presentation;

namespace Kiro.Application
{
    public sealed class PanelRoot
    {
        public PanelRoot(Panel panel, PanelView view, Guid id)
        {
            Panel = panel;
            View = view;
            Id = id;
        }

        public Panel Panel { get; }
        public PanelView View { get; }
        public Guid Id { get; }
    }
}