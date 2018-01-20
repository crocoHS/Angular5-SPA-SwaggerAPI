import { Component, Inject, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../../services/auth.service';
import { TodoService } from '../../services/todo.service';
import { TodoItem } from '../../models/todo-item.type';

@Component({
    selector: 'todo-list',
    templateUrl: './todo-list.component.html',
    styleUrls: ['./todo-list.component.css']
})

export class TodoListComponent {
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    loading = false;
    enableButton = true;
    displayedColumns = ['id', 'name', 'isComplete', 'delete'];
    dataSource: MatTableDataSource<TodoItem>;

    public todos: TodoItem[];
    public newTodo: string = '';
    public userId: string;

    constructor(
        private authService: AuthService,
        private todoService : TodoService,
        @Inject('API_URL') public apiUrl: string) {
    }

    ngOnInit() {
        this.userId = this.authService.getUserId();
        this.getAll();
    }

    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
        this.dataSource.filter = filterValue;
    }

    getAll() {
        this.todoService.getAll()
            .subscribe(todo => {
                this.todos = todo.filter(o => o.ownerId == this.userId);

                this.dataSource = new MatTableDataSource(this.todos);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            });
    }

    completeClicked(todo: TodoItem): void {
        let todoItem = new TodoItem({ id: todo.id, name:todo.name, isComplete: !todo.isComplete, ownerId:todo.ownerId });
        this.todoService.update(todoItem)
            .subscribe();
    }

    newTodoItem(): void {
        if (this.newTodo && this.newTodo.length) {
            this.toggleLoading(false);
            let todoItem = new TodoItem({ name: this.newTodo, isComplete: false, ownerId: this.userId });
            this.todoService.add(todoItem)
                .subscribe((result) => {
                    this.getAll();
                    this.newTodo = '';
                    this.toggleLoading(true);
                });
        }
    }

    deleteItem(id: number): void {
        if (confirm("Are you sure you want to destroy this task forever?")) {
            this.todoService.delete(id)
                .subscribe(() => {
                    this.getAll();
                });
        }
    }

    toggleLoading(isCompleted: boolean) {
        this.enableButton = isCompleted;
        this.loading = !isCompleted;
    }
}