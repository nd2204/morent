import React from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { MoreHorizontal } from "lucide-react";
import { PieChart, Pie, Cell, ResponsiveContainer } from "recharts";
import { CarCategory } from "@/lib/types";

interface TopCarRentalsChartProps {
  categories: CarCategory[];
}

const COLORS = ["#1E40AF", "#1E3A8A", "#2563EB", "#3B82F6", "#93C5FD"];

const TopCarRentalsChart: React.FC<TopCarRentalsChartProps> = ({ categories }) => {
  const totalCount = categories.reduce((acc, category) => acc + category.count, 0);

  // Convert categories to chart data
  const chartData = categories.map((category) => ({
    name: category.name,
    value: category.count,
  }));
  
  return (
    <Card className="bg-white dark:bg-slate-800 rounded-xl shadow-sm">
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle className="font-semibold text-xl">Top 5 Car Rental</CardTitle>
        <button className="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300">
          <MoreHorizontal className="h-5 w-5" />
        </button>
      </CardHeader>
      <CardContent>
        <div className="flex flex-col md:flex-row items-center md:space-x-8">
          {/* Donut Chart */}
          <div className="relative w-[160px] h-[160px] mb-6 md:mb-0">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={chartData}
                  cx="50%"
                  cy="50%"
                  innerRadius={50}
                  outerRadius={70}
                  paddingAngle={1}
                  dataKey="value"
                  stroke="none"
                >
                  {chartData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
              </PieChart>
            </ResponsiveContainer>
            <div className="absolute inset-0 flex flex-col items-center justify-center">
              <span className="text-2xl font-bold dark:text-white">{totalCount.toLocaleString()}</span>
              <span className="text-sm text-gray-500 dark:text-gray-400">Rental Car</span>
            </div>
          </div>
          
          {/* Chart Legend */}
          <div className="flex-1 space-y-2">
            {categories.map((category, index) => (
              <div key={category.id} className="flex items-center justify-between">
                <div className="flex items-center">
                  <span 
                    className="w-2 h-2 rounded-full mr-2" 
                    style={{ backgroundColor: COLORS[index % COLORS.length] }}
                  ></span>
                  <span className="text-sm dark:text-gray-300">{category.name}</span>
                </div>
                <span className="font-medium dark:text-white">{category.count.toLocaleString()}</span>
              </div>
            ))}
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default TopCarRentalsChart;
