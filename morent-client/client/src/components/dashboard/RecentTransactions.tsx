import React from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Transaction } from "@/lib/types";
import { formatDate } from "@/lib/utils";

interface RecentTransactionsProps {
  transactions: Transaction[];
}

const RecentTransactions: React.FC<RecentTransactionsProps> = ({ transactions }) => {
  return (
    <Card className="bg-white dark:bg-slate-800 rounded-xl shadow-sm">
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle className="font-semibold text-xl">Recent Transaction</CardTitle>
        <a href="#" className="text-primary text-sm hover:underline">
          View All
        </a>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {transactions.map((transaction) => (
            <div 
              key={transaction.id} 
              className="flex items-center justify-between py-2 border-b border-gray-100 dark:border-slate-700 last:border-b-0"
            >
              <div className="flex items-center space-x-3">
                <img 
                  src={transaction.car.image} 
                  alt={transaction.car.name} 
                  className="w-14 h-10 object-cover rounded-lg"
                />
                <div>
                  <h4 className="font-medium dark:text-white">{transaction.car.name}</h4>
                  <p className="text-xs text-primary">{transaction.car.category}</p>
                </div>
              </div>
              <div className="text-right">
                <p className="text-sm text-gray-500 dark:text-gray-400">
                  {formatDate(transaction.date)}
                </p>
                <p className="font-semibold dark:text-white">${transaction.amount.toFixed(2)}</p>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
};

export default RecentTransactions;
