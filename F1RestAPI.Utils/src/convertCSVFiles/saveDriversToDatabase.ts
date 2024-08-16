import { Driver, Result } from "../types";
const fs = require("fs");
const csv = require("csv-parser");
const { PrismaClient } = require("@prisma/client");
const path = require("path");

const prisma = new PrismaClient();

const getItems = async () => {
    const driversCSVFile = path.join(__dirname, "dataset", "drivers.csv");
    const resultsCSVFile = path.join(__dirname, "dataset", "results.csv");

    const resultsMap = new Map<string, Result[]>();

    fs.createReadStream(resultsCSVFile)
        .pipe(csv())
        .on("data", (result: Result) => {
            if (!resultsMap.has(result.driverId)) {
                resultsMap.set(result.driverId, []);
            }
            resultsMap.get(result.driverId)?.push(result);
        })
        .on("end", async () => {
            console.log("Results CSV file successfully processed");

            fs.createReadStream(driversCSVFile)
                .pipe(csv())
                .on("data", async (driver: Driver) => {
                    const driverNumberParsed = isNaN(Number(driver.number))
                        ? 0
                        : parseInt(driver.number);
                    const driverResults = resultsMap.get(driver.driverId) || [];
                    let polePositionsCount = 0;
                    let winsCounts = 0;
                    let podiumCount = 0;
                    let DNFCounts = 0;

                    for (const result of driverResults) {
                        if (result.grid === "1") {
                            polePositionsCount++;
                        }

                        if (result.position === "1") {
                            winsCounts++;
                        }

                        if (parseInt(result.position) < 4) {
                            podiumCount++;
                        }

                        if (isNaN(Number(result.position))) {
                            DNFCounts++;
                        }
                    }

                    await prisma.drivers.create({
                        data: {
                            DriverId: parseInt(driver.driverId),
                            DriverRef: driver.driverRef,
                            DriverNumber: driverNumberParsed,
                            DriverCode: driver.code,
                            DriverForename: driver.forename,
                            DriverSurname: driver.surname,
                            DateOfBirth: new Date(driver.dob),
                            Nationality: driver.nationality,
                            WikipediaUrl: driver.url,
                            PolePositions: polePositionsCount,
                            Wins: winsCounts,
                            Podiumns: podiumCount,
                            DNF: DNFCounts,
                        },
                    });
                })
                .on("end", async () => {
                    console.log("Drivers CSV file successfully processed");
                    const drivers = await prisma.drivers.findMany();
                    console.log(drivers.length);
                });
        });
};

getItems();
