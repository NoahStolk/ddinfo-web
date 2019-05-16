-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: devildaggers
-- ------------------------------------------------------
-- Server version	5.7.19-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20190515111757_InitialCreate','2.1.11-servicing-32099'),('20190515114318_V2','2.1.11-servicing-32099'),('20190515115731_V3','2.1.11-servicing-32099'),('20190515135521_V4','2.1.11-servicing-32099'),('20190515153423_V5','2.1.11-servicing-32099');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customentries`
--

DROP TABLE IF EXISTS `customentries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `customentries` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Time` float NOT NULL,
  `Gems` int(11) NOT NULL,
  `Kills` int(11) NOT NULL,
  `DeathType` int(11) NOT NULL,
  `EnemiesAlive` int(11) NOT NULL,
  `Homing` int(11) NOT NULL,
  `LevelUpTime2` float NOT NULL,
  `LevelUpTime3` float NOT NULL,
  `LevelUpTime4` float NOT NULL,
  `CustomLeaderboardID` int(11) NOT NULL,
  `SubmitDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `ShotsFired` int(11) NOT NULL DEFAULT '0',
  `ShotsHit` int(11) NOT NULL DEFAULT '0',
  `Username` longtext,
  `PlayerID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `IX_CustomEntries_CustomLeaderboardID` (`CustomLeaderboardID`),
  CONSTRAINT `FK_CustomEntries_CustomLeaderboards_CustomLeaderboardID` FOREIGN KEY (`CustomLeaderboardID`) REFERENCES `customleaderboards` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customentries`
--

LOCK TABLES `customentries` WRITE;
/*!40000 ALTER TABLE `customentries` DISABLE KEYS */;
INSERT INTO `customentries` VALUES (1,45.9828,4,62,8,591,0,0,0,0,1,'2019-05-15 18:03:47.929678',651,309,'xvlv',21854);
/*!40000 ALTER TABLE `customentries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customleaderboards`
--

DROP TABLE IF EXISTS `customleaderboards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `customleaderboards` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SpawnsetFileName` longtext,
  `SpawnsetHash` longtext,
  `Bronze` float NOT NULL DEFAULT '0',
  `Devil` float NOT NULL DEFAULT '0',
  `Gold` float NOT NULL DEFAULT '0',
  `Silver` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customleaderboards`
--

LOCK TABLES `customleaderboards` WRITE;
/*!40000 ALTER TABLE `customleaderboards` DISABLE KEYS */;
INSERT INTO `customleaderboards` VALUES (1,'AttackAttackAttack_xvlv','98d52c4f70954bd15dc2ea62d40275cf',10,40,30,20),(3,'bintrs cool spawnset_Bintr','acd9e32a9603e5b7b4d97801bc72925d',0,0,0,0);
/*!40000 ALTER TABLE `customleaderboards` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-05-15 21:48:36
