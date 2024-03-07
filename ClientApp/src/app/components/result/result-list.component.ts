import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnChanges, SimpleChange, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "result-list",
  templateUrl: './result-list.component.html',
  styleUrls: ['./result-list.component.css']
})

export class ResultListComponent implements OnChanges {
  @Input() quiz: Quiz | undefined;
  results: Result[] | undefined;
  title: string | undefined;
  baseUrl: string;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router  ) {
    this.results = [];
    this.baseUrl = "https://localhost:7136/api/result/All/"
  }

  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes['quiz'] !== "undefined") {
      // Download information about changes included under key quiz
      var change = changes['quiz'];

      // Perform the task only if the value has changed
      if (!change.isFirstChange()) {
        // Make an http request and retrieve the result
        this.loadData()
      }
    }
  }

  loadData() {
    var url = this.baseUrl + this.quiz?.Id;
    this.http.get<Result[]>(url).subscribe(result => {
      this.results = result;
    }, error => console.error(error));
  }

  onCreate() {
    this.router.navigate(["/result/create", this.quiz?.Id]);
  }

  onEdit(result: Result) {
    this.router.navigate(["/result/edit", result.Id]);
  }

  onDelete(result: Result) {
    if (confirm("Do you really want to delete this result")) {
      var url = this.baseUrl + result.Id;
      this.http
        .delete(url)
        .subscribe(res => {
          console.log("Result " + result.Id + " was deleted.");

          // Refresh the list of results
          this.loadData();
        }, error => console.log(error));
    }
  }
}


