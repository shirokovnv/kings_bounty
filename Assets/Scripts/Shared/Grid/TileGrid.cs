using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Grid
{
    [Serializable]
    public class TileGrid<TGridObject> : ISerializationCallbackReceiver
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private List<TileGridPackage<TGridObject>> packages;

        private TGridObject[,] tiles;

        public TileGrid(
            int width,
            int height,
            Func<TileGrid<TGridObject>, int, int, TGridObject> createGridObject
            )
        {
            this.width = width;
            this.height = height;
            tiles = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = createGridObject(this, x, y);
                }
            }
        }

        public void SetValue(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                tiles[x, y] = value;
            }
        }

        public TGridObject GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return tiles[x, y];
            }

            return default;
        }

        public int GetWidth() { return width; }

        public int GetHeight() { return height; }

        public void OnBeforeSerialize()
        {
            packages = new List<TileGridPackage<TGridObject>>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    packages.Add(new TileGridPackage<TGridObject>(x, y, tiles[x, y]));
                }
            }
        }

        public void OnAfterDeserialize()
        {
            tiles = new TGridObject[width, height];
            foreach (var package in packages)
            {
                tiles[package.X, package.Y] = package.GridObject;
            }

            packages.Clear();
        }
    }
}