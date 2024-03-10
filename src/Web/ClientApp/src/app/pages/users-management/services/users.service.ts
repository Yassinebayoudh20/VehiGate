import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private users: User[] = [
    { id: 1, name: 'John Doe', country: 'USA', company: 'ABC Inc.', representative: 'Alice' },
    { id: 2, name: 'Jane Smith', country: 'Canada', company: 'XYZ Corp.', representative: 'Bob' },
    { id: 3, name: 'Michael Johnson', country: 'UK', company: 'DEF Ltd.', representative: 'Charlie' },
    { id: 4, name: 'Emily Brown', country: 'Australia', company: 'GHI Industries', representative: 'David' },
    { id: 5, name: 'Daniel Wilson', country: 'Germany', company: 'JKL GmbH', representative: 'Emma' },
    { id: 6, name: 'Sarah Taylor', country: 'France', company: 'MNO SA', representative: 'Frank' },
    { id: 7, name: 'James Anderson', country: 'Italy', company: 'PQR Srl', representative: 'Grace' },
    { id: 8, name: 'Olivia Martinez', country: 'Spain', company: 'STU SL', representative: 'Henry' },
    { id: 9, name: 'Liam Hernandez', country: 'Japan', company: 'VWX Co.', representative: 'Isabella' },
    { id: 10, name: 'Sophia Gonzales', country: 'China', company: 'YZ Corp.', representative: 'Jack' },
    { id: 11, name: 'William Lee', country: 'Russia', company: '123 Ltd.', representative: 'Katie' },
    { id: 12, name: 'Emma Kim', country: 'India', company: '456 Inc.', representative: 'Liam' },
    { id: 13, name: 'Noah Nguyen', country: 'Brazil', company: '789 SA', representative: 'Mia' },
    { id: 14, name: 'Ava Patel', country: 'Mexico', company: 'ABC Ltd.', representative: 'Noah' },
    { id: 15, name: 'Ethan Lopez', country: 'Argentina', company: 'XYZ SA', representative: 'Olivia' },
    { id: 16, name: 'Mia Garcia', country: 'South Africa', company: 'DEF Corp.', representative: 'Oscar' },
    { id: 17, name: 'Benjamin Rodriguez', country: 'South Korea', company: 'GHI Co.', representative: 'Penelope' },
    { id: 18, name: 'Isabella Kim', country: 'Nigeria', company: 'JKL Ltd.', representative: 'Quinn' },
    { id: 19, name: 'Alexander Wang', country: 'Egypt', company: 'MNO SA', representative: 'Ryan' },
    { id: 20, name: 'Charlotte Khan', country: 'Turkey', company: 'PQR GmbH', representative: 'Sofia' },
  ];

  constructor() {}
  getUsersLazy(event: any): Observable<{ users: User[]; totalRecords: number }> {
    // Simulate a delayed response
    const filteredUsers = this.filterUsers(this.users, event.filters);
    const sortedUsers = this.sortUsers(filteredUsers, event.sortField, event.sortOrder);

    const startIndex = event.first || 0; // Index of the first record
    const pageSize = event.rows || 10; // Number of records per page
    const paginatedUsers = sortedUsers.slice(startIndex, startIndex + pageSize);

    return of({ users: paginatedUsers, totalRecords: sortedUsers.length });
  }

  private filterUsers(users: User[], filters: any): User[] {
    let filteredUsers = [...users];

    for (const key in filters) {
      if (filters.hasOwnProperty(key)) {
        const filterValue = filters[key].value;
        if (filterValue) {
          filteredUsers = filteredUsers.filter((user) => user[key].toLowerCase().includes(filterValue.toLowerCase()));
        }
      }
    }

    return filteredUsers;
  }

  private sortUsers(users: User[], sortField: string, sortOrder: number): User[] {
    if (sortField && sortOrder) {
      return users.sort((a, b) => {
        const valueA = a[sortField];
        const valueB = b[sortField];
        let comparison = 0;
        if (valueA > valueB) {
          comparison = 1;
        } else if (valueA < valueB) {
          comparison = -1;
        }
        return comparison * sortOrder;
      });
    } else {
      return users;
    }
  }
}
