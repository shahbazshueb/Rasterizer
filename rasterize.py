#!/usr/bin/env python
import math
import os
import urllib
from shapely.geometry import Polygon

def getBoundingBox(polygon):
	bnds=polygon.bounds
	xMin=int(math.floor(bnds[0]))
	xMax=int(math.floor(bnds[2]))
	yMin=int(math.floor(bnds[1]))
	yMax=int(math.floor(bnds[3]))
	return((xMin,xMax),(yMin,yMax))

def isTileIntersecting(regionPolygon, tilePolygon):
	return regionPolygon.intersects(tilePolygon)

def isTileIntersectingCompletely(regionPolygon, tilePolygon):
	return regionPolygon.contains(tilePolygon)

regionPolygon=Polygon([(222.5268,387.6948),(220.1322,389.5893),(222.6205,391.4331),(226.7182,391.4057)])
ranges=getBoundingBox(regionPolygon)

x_range=ranges[0]
y_range=ranges[1]

partialTileList=[]
completeTileList=[]
for y in range(y_range[0], y_range[1]+1):
	for x in range(x_range[0], x_range[1]+1):
		tilePoylgon = Polygon([(x,y), (x+1,y),(x+1,y+1),(x,y+1)])
		if(isTileIntersecting(regionPolygon,tilePoylgon)):
			if(isTileIntersectingCompletely(regionPolygon,tilePoylgon)):
				completeTileList.append((x, y))
			else:
				partialTileList.append((x, y))
tileCount=len(partialTileList)
print str('Partial Tiles: ')
print str(partialTileList)
print str('Complete Tiles: ')
print str(completeTileList)
print 'Total number of Partial Tiles: ' + str(len(partialTileList))
print 'Total number of Complete Tiles: ' + str(len(completeTileList))


