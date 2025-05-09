import React from "react";
import { Card, CardContent } from "@/components/ui/card";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

const sampleData = [
  { month: "Jan", rentals: 65 },
  { month: "Feb", rentals: 59 },
  { month: "Mar", rentals: 80 },
  { month: "Apr", rentals: 81 },
  { month: "May", rentals: 56 },
  { month: "Jun", rentals: 55 },
  { month: "Jul", rentals: 72 },
];

interface StatisticsChartProps {
  title: string;
}

const StatisticsChart: React.FC<StatisticsChartProps> = ({ title }) => {
  return (
    <Card className="bg-white dark:bg-slate-800 rounded-xl shadow-sm">
      <CardContent className="p-6">
        <h2 className="font-semibold text-xl mb-4 dark:text-white">{title}</h2>
        <div className="h-80 w-full">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart
              data={sampleData}
              margin={{
                top: 20,
                right: 30,
                left: 20,
                bottom: 5,
              }}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="rentals" fill="#3B82F6" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
};

export default StatisticsChart;
