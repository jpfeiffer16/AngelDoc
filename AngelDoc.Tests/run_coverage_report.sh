#!/bin/sh

GENERATE_ONLY=""

while getopts ":o" opt; do
  case ${opt} in
    o )
        GENERATE_ONLY="true"
      ;;
    h )
        echo "Usage: run_coverage_report.sh [-o]"
        echo "-o: only generate report, do not open it"
        exit 0
      ;;
  esac
done
shift $((OPTIND -1))

rm -r TestResults
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
dotnet reportgenerator "-reports:./TestResults/*/coverage.opencover.xml" "-targetdir:coveragereport"
if [ -z $GENERATE_ONLY ]
then
    xdg-open ./coveragereport/index.htm
fi
