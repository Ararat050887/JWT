CREATE TABLE `bindings` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `binding_id` varchar(50) NOT NULL,
  `binding_info` varchar(512) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '0',
  `is_confirmed` tinyint(1) NOT NULL DEFAULT '0',
  `merchant_id` int(10) NOT NULL,
  `merchant_user_id` varchar(255) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `cardholder` varchar(265) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `binding_id` (`binding_id`),
  KEY `binding_merchant_id` (`merchant_id`),
  KEY `binding_client_id` (`merchant_user_id`),
  KEY `merchant_id` (`merchant_id`),
  KEY `merchant_id_2` (`merchant_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `merchants` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `client_id` varchar(50) NOT NULL,
  `client_secret` varchar(50) NOT NULL,
  `binded_to` int(10) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `merchant_client_id_unique` (`client_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
