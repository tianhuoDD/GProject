/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 80024
 Source Host           : localhost:3306
 Source Schema         : gpdb

 Target Server Type    : MySQL
 Target Server Version : 80024
 File Encoding         : 65001

 Date: 28/12/2024 14:57:56
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for advertisertb
-- ----------------------------
DROP TABLE IF EXISTS `advertisertb`;
CREATE TABLE `advertisertb`  (
  `advertiserID` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户名（与广告商名称绑定）',
  `ad_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '广告商名称',
  `ad_content` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL COMMENT '广告内容',
  `start_date` datetime(0) NULL DEFAULT NULL COMMENT '广告开始时间',
  `end_date` datetime(0) NULL DEFAULT NULL COMMENT '广告结束时间',
  `status` tinyint(0) NULL DEFAULT 0 COMMENT '广告状态（0为未审核，1为通过，2为播放中，3为已结束）',
  `cost` decimal(10, 2) NULL DEFAULT NULL COMMENT '广告费用',
  `payment_status` tinyint(0) NULL DEFAULT 0 COMMENT '支付状态（0为未支付，1为已支付）',
  `balance` decimal(10, 2) NULL DEFAULT NULL COMMENT '余额',
  PRIMARY KEY (`advertiserID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 25 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for comtb
-- ----------------------------
DROP TABLE IF EXISTS `comtb`;
CREATE TABLE `comtb`  (
  `comID` int(0) NOT NULL,
  `username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '账号/用户名',
  `content` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '评论内容',
  `comtime` datetime(0) NULL DEFAULT NULL COMMENT '评论时间',
  `tvid` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '视频ID',
  `status` tinyint(0) NULL DEFAULT 0 COMMENT '审核状态（0为未发布，1为通过）',
  PRIMARY KEY (`comID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for transaction_recordtb
-- ----------------------------
DROP TABLE IF EXISTS `transaction_recordtb`;
CREATE TABLE `transaction_recordtb`  (
  `transactionID` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ad_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '广告商名称',
  `transaction_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '交易类型（例如：充值、广告消耗）',
  `amount` decimal(10, 2) NOT NULL COMMENT '交易金额',
  `transaction_date` datetime(0) NOT NULL COMMENT '交易时间',
  `alipay_trade_no` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '支付宝交易号',
  `trade_status` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '交易状态',
  PRIMARY KEY (`transactionID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 43 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usertb
-- ----------------------------
DROP TABLE IF EXISTS `usertb`;
CREATE TABLE `usertb`  (
  `id` int(0) NOT NULL,
  `username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `nickname` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `regtime` datetime(0) NULL DEFAULT NULL,
  `icon` mediumblob NULL COMMENT '16MB',
  `admin` tinyint(0) NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for videotb
-- ----------------------------
DROP TABLE IF EXISTS `videotb`;
CREATE TABLE `videotb`  (
  `videoID` int(0) NOT NULL COMMENT '视频id',
  `title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '视频标题',
  `link` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '视频链接',
  `cover` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '视频封面',
  `tag` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '视频标签',
  `year` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '年份',
  `rating` float(3, 1) NULL DEFAULT NULL COMMENT '视频评分',
  `status` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '视频状态（与评分不重合）',
  `hotscore` int(0) NULL DEFAULT NULL COMMENT '视频热度',
  `type` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '视频类型（电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6）',
  `tvid` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '视频唯一标识',
  `desc` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL COMMENT '视频介绍',
  `date` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '上映时间',
  `time` int(0) NULL DEFAULT NULL COMMENT '视频时长',
  `bigcover` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '大封面',
  `director` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '导演',
  `actor` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '演员',
  PRIMARY KEY (`videoID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
