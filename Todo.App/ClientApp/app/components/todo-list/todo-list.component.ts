import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../../services/auth.service';
import { TodoService } from '../../services/todo.service';
import { TodoItem } from '../../models/todo-item.type';

@Component({
    selector: 'todo-list',
    templateUrl: './todo-list.component.html'
})

export class TodoListComponent {
    public todo: TodoItem[];
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

    private getAll() {
        this.todoService.getAll()
            .subscribe(todo => {
                this.todo = todo.filter(o => o.ownerId == this.userId);
            });
    }

    public completeClicked(todo: TodoItem): void {
        let todoItem = new TodoItem({ id: todo.id, name:todo.name, isComplete: !todo.isComplete, ownerId:todo.ownerId });
        this.todoService.update(todoItem)
            .subscribe();
    }

    public newTodoItem(): void {
        if (this.newTodo && this.newTodo.length) {
            let todoItem = new TodoItem({ name: this.newTodo, isComplete: false, ownerId: this.userId });
            this.todoService.add(todoItem)
                .subscribe((result) => {
                    this.getAll();
                    this.newTodo = '';
                });
        }
    }

    public deleteItem(id: number): void {
        if (confirm("Are you sure you want to destroy this task forever?")) {
            this.todoService.delete(id)
                .subscribe(() => {
                    this.getAll();
                });
        }
    }
}