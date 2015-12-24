using System.Collections.Generic;
using Commons.Generics;
using WorldServer.Emu.Models;
using WorldServer.Emu.Structures.Geo.Basics;

namespace WorldServer.Emu.Structures.Geo
{
    public class Area
    {
        private readonly LockedList<ABdoObject> _objects = new LockedList<ABdoObject>();

        public bool PlaceToArea(ABdoObject obj)
        {
            return _objects.AddIfNotPresent(obj);
        }

        public bool RemoveFromArea(ABdoObject obj)
        {
            return _objects.Remove(obj);
        }

        public List<ABdoObject> GetObjectsInRange(Position position, float radius, float minradius = 0)
        {
            var obj = new List<ABdoObject>();

            _objects.Action(element =>
            {
                var distance = element.Position.Distance(position);
                if (distance < radius && distance > minradius)
                    obj.Add(element);
            });

            return obj;
        }
    }
}
