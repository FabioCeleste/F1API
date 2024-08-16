import { Race, Result } from "../types";
const fs = require("fs");
const csv = require("csv-parser");
const { PrismaClient } = require("@prisma/client");
const path = require("path");

const prisma = new PrismaClient();

const getItems = async () => {
    const resultsCSVFile = path.join(__dirname, "dataset", "results.csv");
    const racesCSVFile = path.join(__dirname, "dataset", "races.csv");

    const raceMap = new Map<string, Race[]>();

    fs.createReadStream(racesCSVFile)
        .pipe(csv())
        .on("data", async (race: Race) => {
            if (race.raceId) {
                raceMap.set(race.raceId, []);
            }

            raceMap.get(race.raceId)?.push(race);
        })
        .on("end", async () => {
            fs.createReadStream(resultsCSVFile)
                .pipe(csv())
                .on("data", async (result: Result) => {
                    if (result.driverId == "102") {
                    }

                    const raceResult = raceMap.get(result.raceId) || [];
                    for (const race of raceResult) {
                        try {
                            await prisma.driverConstructors.create({
                                data: {
                                    Year: parseInt(race.year),
                                    ConstructorId: parseInt(
                                        result.constructorId
                                    ),
                                    DriverId: parseInt(result.driverId),
                                },
                            });
                        } catch (error) {
                            console.log("already saved");
                        }
                    }
                })
                .on("end", async () => {
                    console.log("end");
                });
        });
};

getItems();
