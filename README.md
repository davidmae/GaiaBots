# GaiaBots


## Game features 

* Create an ecosystem populate by critters with his own behaviour
* Find the correct balance between aggresive and pacifics critters to accomplish archivements
* Manage the nutriton and many others status for your critters using items for each pourpose
* Exchange your archivements points for getting new items to keeping your ecosystem balanced


## Technical features

* Critters behaviour is implemented with a generic sensor system, which each type of critter has a number of sensors/senses with a specific status to manage
* The sensors/senses are prefabs inside the same critter prefab and each sensor/sense handle a status (health, hungry, etc)
* These sensors can be for "distance" detection with a cone collider or "radius" detection with an overlap sphere. Both could be replace with another detection system onlif inherits from the bases classes (SenseBase, DistanceSense/RadiusSense)
* The sensor/senses system is built as a previous step before the state machine. It is designed for filter all the environment info which is detected by the sensors, leaving the filtered data to the state machine. 


