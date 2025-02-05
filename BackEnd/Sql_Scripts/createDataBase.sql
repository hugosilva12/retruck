USE [ReTruck]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Absence]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Absence](
	[id] [uniqueidentifier] NOT NULL,
	[driverid] [uniqueidentifier] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[absence] [int] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_Absence] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[License]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[License](
	[id] [uniqueidentifier] NOT NULL,
	[driverid] [uniqueidentifier] NOT NULL,
	[truck_category] [int] NOT NULL,
 CONSTRAINT [PK_License] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[enable] [bit] NOT NULL,
	[addresses] [nvarchar](max) NOT NULL,
	[vatin] [int] NOT NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PathPhoto]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PathPhoto](
	[number] [int] IDENTITY(1,1) NOT NULL,
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[type] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PathPhoto] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[id] [uniqueidentifier] NOT NULL,
	[idTruck] [nvarchar](max) NOT NULL,
	[idTransport] [nvarchar](max) NOT NULL,
	[status] [int] NOT NULL,
	[kms] [float] NOT NULL,
	[profit] [float] NOT NULL,
	[capacityAvailable] [float] NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCoord]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCoord](
	[id] [uniqueidentifier] NOT NULL,
	[serviceTransportid] [uniqueidentifier] NOT NULL,
	[latitude] [float] NOT NULL,
	[longitude] [float] NOT NULL,
 CONSTRAINT [PK_ServiceCoord] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transport]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transport](
	[id] [uniqueidentifier] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[truck_category] [int] NOT NULL,
	[origin] [nvarchar](max) NOT NULL,
	[destiny] [nvarchar](max) NOT NULL,
	[weight] [float] NOT NULL,
	[capacity] [float] NOT NULL,
	[liters] [int] NOT NULL,
	[value_offered] [float] NOT NULL,
	[user_clientid] [uniqueidentifier] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_Transport] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransportReviewParameters]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransportReviewParameters](
	[id] [uniqueidentifier] NOT NULL,
	[valueSaturday] [float] NOT NULL,
	[valueSunday] [float] NOT NULL,
	[valueHoliday] [float] NOT NULL,
	[valueFuel] [float] NOT NULL,
	[valueToll] [float] NOT NULL,
	[typeAnalysis] [int] NOT NULL,
	[considerTruckBreakDowns] [int] NOT NULL,
 CONSTRAINT [PK_TransportReviewParameters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Truck]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Truck](
	[id] [uniqueidentifier] NOT NULL,
	[driverid] [uniqueidentifier] NOT NULL,
	[matricula] [nvarchar](max) NOT NULL,
	[year] [int] NOT NULL,
	[truckCategory] [int] NOT NULL,
	[fuelConsumption] [int] NOT NULL,
	[kms] [int] NOT NULL,
	[nextRevision] [int] NOT NULL,
	[photoPath] [nvarchar](max) NOT NULL,
	[organization_id] [nvarchar](max) NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_Truck] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TruckBreakDowns]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TruckBreakDowns](
	[id] [uniqueidentifier] NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[truckid] [uniqueidentifier] NOT NULL,
	[price] [float] NOT NULL,
 CONSTRAINT [PK_TruckBreakDowns] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 29/08/2022 23:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [uniqueidentifier] NOT NULL,
	[organizationid] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](max) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[email] [nvarchar](max) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
	[role] [int] NOT NULL,
	[photofilename] [nvarchar](max) NOT NULL,
	[userState] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Service] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [capacityAvailable]
GO
ALTER TABLE [dbo].[Absence]  WITH CHECK ADD  CONSTRAINT [FK_Absence_User_driverid] FOREIGN KEY([driverid])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Absence] CHECK CONSTRAINT [FK_Absence_User_driverid]
GO
ALTER TABLE [dbo].[License]  WITH CHECK ADD  CONSTRAINT [FK_License_User_driverid] FOREIGN KEY([driverid])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[License] CHECK CONSTRAINT [FK_License_User_driverid]
GO
ALTER TABLE [dbo].[ServiceCoord]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCoord_Service_serviceTransportid] FOREIGN KEY([serviceTransportid])
REFERENCES [dbo].[Service] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceCoord] CHECK CONSTRAINT [FK_ServiceCoord_Service_serviceTransportid]
GO
ALTER TABLE [dbo].[Transport]  WITH CHECK ADD  CONSTRAINT [FK_Transport_User_user_clientid] FOREIGN KEY([user_clientid])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transport] CHECK CONSTRAINT [FK_Transport_User_user_clientid]
GO
ALTER TABLE [dbo].[Truck]  WITH CHECK ADD  CONSTRAINT [FK_Truck_User_driverid] FOREIGN KEY([driverid])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Truck] CHECK CONSTRAINT [FK_Truck_User_driverid]
GO
ALTER TABLE [dbo].[TruckBreakDowns]  WITH CHECK ADD  CONSTRAINT [FK_TruckBreakDowns_Truck_truckid] FOREIGN KEY([truckid])
REFERENCES [dbo].[Truck] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TruckBreakDowns] CHECK CONSTRAINT [FK_TruckBreakDowns_Truck_truckid]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Organization_organizationid] FOREIGN KEY([organizationid])
REFERENCES [dbo].[Organization] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Organization_organizationid]
GO
