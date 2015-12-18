/*
  That file part of Code Monsters framework.
  Cerium Unity 2015 © 
*/
namespace LoginServer.Emu.Interfaces
{
    public interface IProcessor
    {
        void OnLoad(object previousInstanceContext);
        object OnUnload();
    }
}
