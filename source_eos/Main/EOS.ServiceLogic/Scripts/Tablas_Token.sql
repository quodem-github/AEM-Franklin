
/*
CREATE TABLE q_service
(
	IdService 		INT NOT NULL AUTO_INCREMENT,
    ServiceCode 	nvarchar(50),
    Title			nvarchar(50),
    Status			bit,
    CreationDate	datetime,    
    PRIMARY KEY (IdService)   
    
);


CREATE TABLE q_suscriptor
(
	IdSuscriptor 	INT NOT NULL AUTO_INCREMENT,
    PublicKey	  	nvarchar(255),
    PrivateKey		nvarchar(255),
    Title			nvarchar(255),
    Status			bit,
    CreationDate	datetime,
    PRIMARY KEY (IdSuscriptor)    
);

CREATE TABLE q_service_suscriptor
(
	IdServiceSuscriptor INT NOT NULL AUTO_INCREMENT,
    IdService int,
    IdSuscriptor int,
	PRIMARY KEY (IdServiceSuscriptor),    
	CONSTRAINT `FK_q_subcriptor_q_service_suscriptor` FOREIGN KEY (IdSuscriptor) REFERENCES `q_suscriptor` (`IdSuscriptor`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `FK_q_service_q_service_suscriptor` FOREIGN KEY (IdService) REFERENCES `q_service` (`IdService`) ON DELETE NO ACTION ON UPDATE NO ACTION
    
);


CREATE TABLE q_service_query
(
	IdServiceQuery 		INT NOT NULL AUTO_INCREMENT,
    IdServiceSuscriptor		INT,
    HashedKey			nvarchar(100),
    CreateDate			datetime,
    UpdateDate datetime,
    RandonGuid			nvarchar(50),
    PRIMARY KEY (IdServiceQuery) ,
    CONSTRAINT `FK_q_subcriptor_q_service_query` FOREIGN KEY (IdServiceSuscriptor) REFERENCES `q_service_suscriptor` (`IdServiceSuscriptor`) ON DELETE NO ACTION ON UPDATE NO ACTION    
);



CREATE TABLE q_service_statistics(
	IServiceStatistic INT NOT NULL AUTO_INCREMENT,	
	IdSuscriptor int NULL,
	IdService int NULL,
	Method nvarchar(50) NULL,
	Params text NULL,
    CreatedDate datetime NOT NULL,	
	ResultStatus int NOT NULL,
    PRIMARY KEY (IServiceStatistic),    
    CONSTRAINT `FK_q_subcriptor_q_service_statistics` FOREIGN KEY (IdSuscriptor) REFERENCES `q_suscriptor` (`IdSuscriptor`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `FK_q_service_q_service_statistics` FOREIGN KEY (IdService) REFERENCES `q_service` (`IdService`) ON DELETE NO ACTION ON UPDATE NO ACTION 
) ;



INSERT INTO q_service(`IdService`,`ServiceCode`,`Title`,`Status`,`CreationDate`)VALUES(1,'ServiceEOS','Service EOS',1,'2016-05-25');

INSERT INTO q_suscriptor(`IdSuscriptor`,`PublicKey`,`PrivateKey`,`Title`,`Status`,`CreationDate`)VALUES(1,'EosPubkey','EosPrivKey__24Characters','Service Default Suscriptor EOS',1,'2016-05-25');

INSERT INTO q_service_suscriptor(`IdServiceSuscriptor`,`IdService`,`IdSuscriptor`)VALUES(1,1,1);

*/


