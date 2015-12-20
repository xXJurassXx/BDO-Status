/*
  That file part of Code Monsters framework.
  Cerium Unity 2015 © 
*/
namespace WorldServer.Emu.Interfaces
{
    public interface IProcessor
    {
        void OnLoad(object previousInstanceContext);
        object OnUnload();
    }
}
