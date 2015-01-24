using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathFinder
{
    class RoomNode
    {
       public Room room;
       public RoomNode parent;
    }

    public static Stack<Room> FindPath(Room start, Room target)
    {
        Queue<RoomNode> queue = new Queue<RoomNode>();
        List<Room> visited = new List<Room>();
        Stack<Room> path = new Stack<Room>();

        RoomNode root = new RoomNode();
        root.room = start;
        root.parent = null;

        visited.Add(start);
        queue.Enqueue(root);

        bool foundPath = false;
        RoomNode curNode = root;
        while(!foundPath && queue.Count > 0)
        {
            curNode = queue.Dequeue();

            if (curNode.room == target)
            {
                foundPath = true;
            }

            foreach(Room r in curNode.room.m_neighbours)
            {
                if (!visited.Contains(r))
                {
                    visited.Add(r);

                    if (r.Status != Room.RoomEmergency.DESTRoYED || r == target)
                    {
                        RoomNode newNode = new RoomNode();
                        newNode.room = r;
                        newNode.parent = curNode;

                        queue.Enqueue(newNode);
                    }
                }
            }
        }

        //build path
        while(foundPath && curNode != null)
        {
            path.Push(curNode.room);
            curNode = curNode.parent;
        }

        return path;
    }
}
