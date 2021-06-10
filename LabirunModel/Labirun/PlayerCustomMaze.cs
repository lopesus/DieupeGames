using System;
using System.Collections.Generic;
using LabirunModel.Tools;

namespace LabirunModel.Labirun
{
    public class PlayerCustomMaze
    {
        public int Count { get; set; }
        public Dictionary<string, CreatedMaze> PlayerMazes { get; set; }

        public PlayerCustomMaze()
        {
            PlayerMazes = new Dictionary<string, CreatedMaze>();
        }

        public void AddMaze(CreatedMaze maze)
        {
            if (maze?.Id != null)
            {
                maze.UpdateTime = DateTime.UtcNow;
                PlayerMazes[maze.Id] = maze;
                Count++;
            }
        }
        public void UpdateMaze(CreatedMaze maze)
        {
            if (maze?.Id != null)
            {
                maze.UpdateTime = DateTime.UtcNow;
                PlayerMazes[maze.Id] = maze;
            }
        }

        public void DeleteMaze(CreatedMaze maze)
        {
            if (maze?.Id != null)
            {
                PlayerMazes.Remove(maze.Id);
            }
        }
        public CreatedMaze GetMaze(string id)
        {
            if (id.IsEmptyString() == false)
            {
                PlayerMazes.TryGetValue(id, out var maze);
                return maze;
            }

            return null;
        }
    }
}