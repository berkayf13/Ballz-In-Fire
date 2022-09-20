﻿using System.Threading.Tasks;
using F13StandardUtils.CbkFramework.Scripts.Core.Command;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Example
{
    public class C_ExampleCommand : BaseCommand
    {
        protected override async void OnExecute(IEvent e = null)
        {
            await Task.Delay(1000);
            LogWarning("ExampleCommand3 OnExecute");
            Complete();
        }
    }
}