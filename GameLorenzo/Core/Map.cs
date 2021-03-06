﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    //genereren van de tile map
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
            set { collisionTiles = value; }
        }

        private int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        
        public Map() { }

        public void Generate(int[,] map, int size)
        {
            
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    int number = map[j, i];
                    if (number > 0)
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(i * size, j * size, size, size)));

                    width = (i + 1) * size;
                    height = (j + 1) * size;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
        }
    }
    //tile map designs in mdArrays
    class MapLevelGenerator
    {
        
        public void LoadContent(int level, Map map)
        {
            if (level == 1)
            {
                map.CollisionTiles.Clear();
                map.Generate(new int[,]{
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    {1,1,1,1,0,0,0,1,0,0,0,1,1,0,0,0,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                    {1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,1},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                    {1,1,0,0,0,0,1,0,0,0,1,0,0,1,0,0,0,0},
                    {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                }, 80);
            }
            else if (level == 2)
            {
                map.CollisionTiles.Clear();
                map.Generate(new int[,]{
                {3,3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,4,0,0,0,4,0,3,3,3,3},
                {3,3,3,3,3,4,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,0,0,3,0,4,0,0,0,4,0,0,0,3,3,3,3},
                {3,3,0,0,0,0,0,0,0,0,0,0,0,4,3,3,3,3},
                {3,3,0,0,3,0,0,0,4,0,0,0,4,0,3,3,3,3},
                {3,3,3,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,3,3,0,4,0,0,0,4,0,0,0,3,3,3,3},
                {3,3,3,3,0,0,0,0,0,0,0,0,0,4,3,3,3,3},
                {3,0,0,0,0,0,0,4,0,0,0,4,0,0,3,3,3,3},
                {3,0,0,0,3,4,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,0,0,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,0,0,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,0,0,0,0,4,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,3,3,0,0,0,0,0,4,0,0,0,3,3,3,3},
                {3,3,3,3,3,0,0,0,0,0,0,0,0,4,3,3,3,3},
                {3,3,3,3,3,0,0,0,0,0,4,0,0,0,3,3,3,3},
                {3,0,0,0,0,0,0,4,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                }, 80);
                
            }
        }
    }
}
